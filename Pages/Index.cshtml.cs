using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using aspnetfeatures.Middlewares;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace aspnetfeatures.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> logger;
        public Clock Clock { get; private set; }
        public TimeZoneInfo TimeZone { get; set; }
        
        [BindProperty]
        public string TimeZoneId { get; set; }

        public List<SelectListItem> TimeZones =>
            TimeZoneInfo.GetSystemTimeZones()
                .Select(tz => new SelectListItem
                {
                    Text = $"{tz.Id} ({tz.DisplayName})",
                    Value = tz.Id
                })
                .ToList();

        public IndexModel(ILogger<IndexModel> logger)
        {
            this.logger = logger;
        }

public void OnGet()
{
    Clock = HttpContext.Features.Get<Clock>();
    TimeZone = Clock.TimeZone;
    TimeZoneId = Clock.TimeZone.Id;
}

public IActionResult OnPost()
{
    ClockMiddleware.SetTimeZone(Response, TimeZoneId);
    return RedirectToPage("Index");
}
    }
}
