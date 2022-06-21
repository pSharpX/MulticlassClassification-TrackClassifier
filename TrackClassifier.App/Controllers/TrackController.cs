using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrackClassifier.App.Models;
using TrackClassifier.App.Services;

namespace TrackClassifier.App.Controllers
{
    [ApiController]
    [Route("Track")]
    public class TrackController : ControllerBase
    {
        private readonly ILogger<TrackController> _logger;
        private readonly ITrackSuggestionService _service;

        public TrackController(ILogger<TrackController> logger,
            ITrackSuggestionService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet(Name = "Greetings")]
        [Route("greeting/{name}")]
        public string Get(string name)
        {
            return $"Hello {name} !";
        }

        [HttpPost(Name = "Track-Suggestion")]
        [Route("suggestion")]
        public TrackSuggestionResponse GetTrackSuggestion([FromBody] AssessmentScoringRequest request)
        {
            return _service.Recommend(request);
        }
    }
}
