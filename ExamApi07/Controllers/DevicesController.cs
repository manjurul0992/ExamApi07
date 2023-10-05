using ExamApi07.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace ExamApi07.Controllers
{

    public class DevicesController : ApiController
    {

        private DeviceDbContext db = new DeviceDbContext();

        [BasicAuthentication]
        [HttpGet]

        public IQueryable<Device> GetDevices()
        {
            return db.Devices.Include(x => x.Specs).AsQueryable();
        }
        
        [HttpGet]
        public IHttpActionResult GetDevice(int id)
        {
            var d = db.Devices.Include(x => x.Specs).FirstOrDefault(x => x.DeviceId == id);
            if (d != null)
            {
                return Ok(d);
            }
            else
            {
                return NotFound();
            }
        }
        [HttpPost]
        public IHttpActionResult PostDevice(DeviceInputModel model)
        {
            if (ModelState.IsValid)
            {
                var device = new Device
                {
                    DeviceName = model.DeviceName,
                    ReleaseDate = model.ReleaseDate,
                    Price = model.Price,
                    OnSale = model.OnSale,
                    Picture = model.Picture
                };
                model.Specs.ForEach(s =>
                {
                    device.Specs.Add(new Spec { SpecName = s.SpecName, Value = s.Value });
                });
                db.Devices.Add(device);
                db.SaveChanges();
                return Ok(device);
            }
            return BadRequest("Data Invalid!!!");
        }
        [Route("Image/Upload")]
        [HttpPost]
        public IHttpActionResult Upload()
        {
            var file = HttpContext.Current.Request.Files.Count > 0 ? HttpContext.Current.Request.Files[0] : null;
            if (file != null && file.ContentLength > 0)
            {
                string ext = Path.GetExtension(file.FileName);
                string f = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ext;
                string savePath = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Images"), f);
                file.SaveAs(savePath);
                return Ok(f);
            }
            return BadRequest();
        }
        [HttpPut]
        public IHttpActionResult PutDevice(int id, DeviceInputModel model)
        {
            if (id != model.DeviceId) return BadRequest("Id mismatch");
            if (ModelState.IsValid)
            {
                var device = db.Devices.Include(x => x.Specs).First(x => x.DeviceId == id);
                if (device == null) return NotFound();
                device.DeviceName = model.DeviceName;
                device.ReleaseDate = model.ReleaseDate;
                device.Price = model.Price;
                device.OnSale = model.OnSale;
                device.Picture = model.Picture;
                db.Specs.RemoveRange(device.Specs);
                model.Specs.ForEach(s =>
                {
                    device.Specs.Add(new Spec { SpecName = s.SpecName, Value = s.Value });
                });
                db.SaveChanges();
                return Ok(device);
            }
            return BadRequest("Data Invalid!!");
        }
        
        [HttpDelete]
        public IHttpActionResult DeleteDevice(int id)
        {
            var d = db.Devices.FirstOrDefault(x => x.DeviceId == id);
            if (d == null)
                return NotFound();

           
            string filename = d.Picture;

            
            db.Devices.Remove(d);
            db.SaveChanges();

            
            if (!string.IsNullOrEmpty(filename))
            {
                string imagePath = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Images"), filename);
                if (File.Exists(imagePath))
                {
                    File.Delete(imagePath);
                }
            }

            return Ok("Deleted");
        }
    }

}

