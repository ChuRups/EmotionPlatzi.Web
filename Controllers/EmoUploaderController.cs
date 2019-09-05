using EmotionPlatzi.Web.Models;
using EmotionPlatzi.Web.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EmotionPlatzi.Web.Controllers
{
    public class EmoUploaderController : Controller
    {
        string serverFolderPath;
        EmotionHelper emoHelper;
        string key;
        EmotionPlatziWebContext db = new EmotionPlatziWebContext();

        public EmoUploaderController()
        {            
            serverFolderPath = ConfigurationManager.AppSettings["UPLOAD_DIR"];
            key = ConfigurationManager.AppSettings["EMOTION_KEY"];
            emoHelper = new EmotionHelper(key);
        }

        // GET: EmoUploader
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Index(HttpPostedFileBase file)
        {
            if (file?.ContentLength > 0)//file != null && file.ContentLength > 0)
            {
                var pictureName = Guid.NewGuid().ToString(); //No se guarda con el Nombre de la imagen porque se puede sobreescribir, se genera un numero aleatorio.
                pictureName += Path.GetExtension(file.FileName); //se obtiene la extension del archivo

                var route = Server.MapPath(serverFolderPath); //mapea una ruta de servidor a una ruta local
                route = $"{route}\\{pictureName}";
                file.SaveAs(route);

                var emoPicture = await emoHelper.DetectAndExtracFacesAsync(file.InputStream);
                emoPicture.Name = file.FileName;
                emoPicture.Path = $"{serverFolderPath}/{pictureName}";
                db.EmoPictures.Add(emoPicture);
                await db.SaveChangesAsync();
                return RedirectToAction("Details","EmoPictures", new { Id = emoPicture.Id});
            }
            return View();
        }
    }
}