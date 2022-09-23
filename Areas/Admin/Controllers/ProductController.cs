using BookShop.Data.Repository.IRepository;
using BookShop.Models;
using BookShop.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnviroment;


        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnviroment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnviroment = webHostEnviroment;
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
            /* from a view model*/
            ProductVM productVM = new ProductVM
            {
                Product = new Product(),

                CategoryList = new SelectList(_unitOfWork.Category.GetAll(), "Id", "Name"),

                CoverTypeList = _unitOfWork.CoverType.GetAll().Select(i => new SelectListItem
                {
                    Value = i.Id.ToString(),
                    Text = i.Name,
                })
            };


            /*    Product product = new();
                *//* Get drop down list Using View Data *//*
                ViewData["CategoryList"] = new SelectList(_unitOfWork.Category.GetAll(), "Id", "Name");*/

            /* get drop down Using projections with Select*/
            /*  IEnumerable<SelectListItem> CoverTypeList = _unitOfWork.CoverType.GetAll().Select(
                   u => new SelectListItem
                   {
                       Text = u.Name,
                       Value = u.Id.ToString()
                   });
  */
            if (id == null || id == 0)
            {// create the product

                /*ViewBag.CoverType = CoverTypeList;*/
                ;
                return View(productVM);
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
        // have to add iformfile to capture an upload
        public IActionResult Upsert(ProductVM obj, IFormFile file)
        {


            if (ModelState.IsValid)
            {
                // gets the path to the wwwRoot
                string rootPath = _webHostEnviroment.WebRootPath;

                if(file != null) 
                {
                    // creates unique name for file
                    string fileName = Guid.NewGuid().ToString();

                    // Combines the wwwRoot path with the folders location
                    var uploads = Path.Combine(rootPath, @"Images\Products");

                    // Gets the file extension from the file passed into upsert
                    var extension = Path.GetExtension(file.FileName);

                    // Createing a new file stream to manipulate files. combine the paths then create a new file
                    var fileStream = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create);

                    // using statement copys the uploaded file to the target stream
                    using (fileStream)
                    {
                        file.CopyTo(fileStream);
                    }

                    // Add the path to the image to the database
                    obj.Product.ImageUrl = @"\Images\Products\" + fileName + extension;
                
                }

                _unitOfWork.Product.Add(obj.Product);
                _unitOfWork.Save();
                TempData["success"] = "New Product Added Successfully";

                return RedirectToAction(nameof(Index));
            }
            return View();
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



        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var productList = _unitOfWork.Product.GetAll();

            return Json(new {data = productList});

        }

        #endregion

    }
}

