using BookShop.Data.Repository.IRepository;
using BookShop.Models;
using BookShop.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
     


        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        
        }

        // GET: Categories
        public IActionResult Index()
        {
            
            return View();
        
        }



        // GET: Categories/Edit/5
        public IActionResult Upsert(int? id)
        {
            /* from a view model*/
            Company company = new Company();
           


            if (id == null || id == 0)
            {
                return View(company);
            }
            else
            {
                company = _unitOfWork.Company.GetFirstOrDefault(c => c.Id == id);

                return View(company);

            }


         
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        // have to add iformfile to capture an upload
        public IActionResult Upsert(Company obj)
        {


            if (ModelState.IsValid)
            {
 

                if(obj.Id == 0)
                {
                    _unitOfWork.Company.Add(obj);
                    TempData["success"] = "New Company Added Successfully";

                }
                else
                {
                    _unitOfWork.Company.Update(obj);
                    TempData["success"] = "Company Updated Successfully";

                }
                _unitOfWork.Save();

               
                
                

                return RedirectToAction(nameof(Index));
            }
            return View();
        }

  
        #region API CALLS
        
        [HttpGet]
        public IActionResult GetAll()
        {
            var companyList = _unitOfWork.Company.GetAll();

            return Json(new {data = companyList});

        }

        // POST: Categories/Delete/5
        [HttpDelete]
        public IActionResult DeleteConfirmed(int? id)
        {
          
            var company = _unitOfWork.Company.GetFirstOrDefault(u => u.Id == id);

            if (company != null)
            {
               
                _unitOfWork.Company.Remove(company);
            } else
            {
                return Json(new {success = false, message = "Error while deleting"});
            }

            _unitOfWork.Save();
           
            return Json(new {success = true, message = "Product deleted successfully"});
        }


        #endregion

    }
}

