using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace TreeViewDemo.Views.Home
{
    public class Demo : PageModel
    {
        private readonly ILogger<Demo> _logger;

        public Demo(ILogger<Demo> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}