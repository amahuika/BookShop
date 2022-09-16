using BookShop.Data.Repository.IRepository;
using BookShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Categories
        public IActionResult Index()
        {
            IEnumerable<Product> ObjCoverTypeList = _unitOfWork.Product.GetAll();
            return View(ObjCoverTypeList);
            /*     return _context.Category != null ? 
                             View(await _context.Category.ToListAsync()) :
                             Problem("Entity set 'ApplicationDbContext.Category'  is null.");*/
        }

      

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Name")] CoverType coverType)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.CoverType.Add(coverType);
                _unitOfWork.Save();
                TempData["success"] = "Cover created successfully";

                return RedirectToAction(nameof(Index));
            }
            return View(coverType);
        }

        // GET: Categories/Edit/5
        public IActionResult Upsert(int? id)
        {
            Product product = new();
            /* Get drop down list Using View Data */
            ViewData["CategoryList"] = new SelectList(_unitOfWork.Category.GetAll(), "Id", "Name");

                 /* get drop down Using projections with Select*/
           IEnumerable<SelectListItem> CoverTypeList = _unitOfWork.CoverType.GetAll().Select(
                u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });

            if (id == null || id == 0)
            {// create the product

                ViewBag.CoverType = CoverTypeList;
                    ;
                return View(product);
            }
            else
            {

            }

       
            return View();
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(int id, [Bind("Id,Name")] CoverType coverType)
        {
            if (id != coverType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                _unitOfWork.CoverType.Update(coverType);
                _unitOfWork.Save();
                TempData["success"] = "Cover updated successfully";

                return RedirectToAction(nameof(Index));
            }
            return View(coverType);
        }

        // GET: Categories/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coverType = _unitOfWork
                .CoverType.GetFirstOrDefault(m => m.Id == id);
            if (coverType == null)
            {
                return NotFound();
            }

            return View(coverType);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (_unitOfWork == null)
            {
                return Problem("Entity set 'Cover'  is null.");
            }
            var coverType = _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id);
            if (coverType != null)
            {
                _unitOfWork.CoverType.Remove(coverType);
            }

            _unitOfWork.Save();
            TempData["success"] = "Cover deleted successfully";
            return RedirectToAction(nameof(Index));
        }

        /*  private bool CategoryExists(int id)
          {
            return (_context.Category?.Any(e => e.Id == id)).GetValueOrDefault();
          }*/
    }
}

