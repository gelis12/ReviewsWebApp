using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReviewsWebApp.Data;
using ReviewsWebApp.Models;

namespace ReviewsWebApp.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ReviewsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews
                .Include(r => r.CreatedBy)
                .FirstOrDefaultAsync(m => m.Id == id);

            var user = await _userManager.GetUserAsync(User);

            var reviewViewModel = ReviewViewModel.FromEntity(review);

            reviewViewModel.HasEditAndDeletePermissions = string.Equals(user?.Id, reviewViewModel.CreatedById);

            if (review == null)
            {
                return NotFound();
            }

            return View(reviewViewModel);
        }

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReviewViewModel reviewViewModel)
        {
            
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);



                var review = reviewViewModel.ToEntity();
                review.Id = Guid.NewGuid();
                
                review.Created = DateTime.Now;
                review.Modified = DateTime.Now;
                review.CreatedBy = user;


                _context.Add(review);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index","Home");
            }
            return View(reviewViewModel);
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews.FindAsync(id);

            if (review == null)
            {
                return NotFound();
            }

            var reviewViewModel = ReviewViewModel.FromEntity(review);

            return View(reviewViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ReviewViewModel reviewViewModel)
        {
            if (id != reviewViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var review = reviewViewModel.ToEntity();

                    review.Modified = DateTime.Now;

                    _context.Update(review);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReviewExists(reviewViewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Home");
            }
            return View(reviewViewModel);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews
                .FirstOrDefaultAsync(m => m.Id == id);
            if (review == null)
            {
                return NotFound();
            }

            var reviewViewModel = ReviewViewModel.FromEntity(review);

            return View(reviewViewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review != null)
            {
                _context.Reviews.Remove(review);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        private bool ReviewExists(Guid id)
        {
            return _context.Reviews.Any(e => e.Id == id);
        }
    }
}
