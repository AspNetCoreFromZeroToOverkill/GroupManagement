using System.Collections.Generic;
using System.Linq;
using CodingMilitia.PlayBall.GroupManagement.Web.Demo;
using CodingMilitia.PlayBall.GroupManagement.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace CodingMilitia.PlayBall.GroupManagement.Web.Controllers
{
    [Route("groups")]
    public class GroupsController : Controller
    {
        private static List<GroupViewModel> groups = new List<GroupViewModel> { new GroupViewModel { Id = 1, Name = "Sample Group"} };
        
        private readonly IGroupIdGenerator _groupIdGenerator;
        
        public GroupsController(IGroupIdGenerator groupIdGenerator)
        {
            _groupIdGenerator = groupIdGenerator;
        }
        
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return View(groups);
        }
        
        [HttpGet]
        [Route("{id}")]
        public IActionResult Details(long id)
        {
            var group = groups.SingleOrDefault(g => g.Id == id);

            if (group == null)
            {
                return NotFound();
            }

            return View(group);
        }

        [HttpPost]
        [Route("{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(long id, GroupViewModel model)
        {
            var group = groups.SingleOrDefault(g => g.Id == id);

            if (group == null)
            {
                return NotFound();
            }

            group.Name = model.Name;

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("")]
        [ValidateAntiForgeryToken]
        public IActionResult CreateReally(GroupViewModel model)
        {
            model.Id = _groupIdGenerator.Next();
            groups.Add(model);

            return RedirectToAction("Index");
        }
    }
}