using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NoteApi7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkoutController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public WorkoutController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public class Workout
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Reps { get; set; }
            public string Weight { get; set; }
        }


        [HttpGet] 
        public async Task<IActionResult> GetData()
        {

            var response = await _httpClient.GetAsync("https://marcushnodeapi.azurewebsites.net/log");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("There was an error");
            }
            
            var data = await response.Content.ReadAsStringAsync();
            
            return Ok(data);
        }

        [HttpPost]
        public async Task<ActionResult<string>> PostWorkout(string id, string name, string reps, string weight)
        {
            var workout = new Workout()
            {
                Id = id,
                Name = name,
                Reps = reps,
                Weight = weight
            };

            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Post, "https://marcushnodeapi.azurewebsites.net/add")
                {
                    Content = new StringContent($"id={workout.Id}&name={workout.Name}&reps={workout.Reps}&weight={workout.Weight}", Encoding.UTF8, "application/x-www-form-urlencoded")
                };

                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadAsStringAsync();
                return result;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkout(string id)
        {
            var response = await _httpClient.DeleteAsync($"https://marcushnodeapi.azurewebsites.net/delete/{id}");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("There was an error");
            }

            return Ok();
        }

    }
}
