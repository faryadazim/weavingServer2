using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using DAL;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using test6EntityFrame.Models; 

namespace test6EntityFrame.Controllers
{
    public class UsersController : ApiController
    {
        private ApplicationUserManager _userManager;
        private db_weavingEntities db = new db_weavingEntities();
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: api/Users
        [Route("api/Users")]
   
        public IHttpActionResult GetAspNetUsers()
        {
        var data= db.AspNetUsers;
            var user = (from urole in db.AspNetUserRoles
                        join userdb in db.AspNetUsers on urole.UserId equals userdb.Id
                        from role in db.AspNetRoles
                        where role.Id == urole.RoleId
                        select new
                        {
                            id = userdb.Id,
                            email = userdb.Email,
                            userName = userdb.UserName,
                            role = urole.RoleId,
                            roleName = role.Name
                        });
            return Ok(user);
        }

        // GET: api/Users/5
        [ResponseType(typeof(AspNetUsers))]
        public IHttpActionResult GetAspNetUsers(string id)
        {
            AspNetUsers aspNetUsers = db.AspNetUsers.Find(id);
            if (aspNetUsers == null)
            {
                return NotFound();
            }

            return Ok(aspNetUsers);
        }

        // PUT: api/Users/5 
        [Route("api/Users")]
        public IHttpActionResult PutUser(string InputId, string InputuserName , string Inputemail )
        {
            //var entity = entities.device_config.FirstOrDefault(e => e.device_config_id == 1);
            var entity = db.AspNetUsers.FirstOrDefault(e => e.Id == InputId);
            if (entity == null)
            {
                return NotFound();
            }
            else
            { 
                entity.Email = Inputemail;
                entity.UserName = InputuserName; 
                db.SaveChanges();
            //    //entities.SaveChanges(); 
            }
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            //if (id != aspNetUsers.Id)
            //{
            //    return BadRequest();
            //}

            //db.Entry(aspNetUsers).State = EntityState.Modified;

            //try
            //{
            //    db.SaveChanges();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!AspNetUsersExists(id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}
          
            return Ok();
        }

        // POST: api/Users
        //[ResponseType(typeof(AspNetUsers))]
        //public IHttpActionResult PostAspNetUsers(AspNetUsers aspNetUsers)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.AspNetUsers.Add(aspNetUsers);

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateException)
        //    {
        //        if (AspNetUsersExists(aspNetUsers.Id))
        //        {
        //            return Conflict();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return CreatedAtRoute("DefaultApi", new { id = aspNetUsers.Id }, aspNetUsers);
        //}

             
        [Route("api/Users")]
        public IHttpActionResult async(UserDataModel model)
       // public async Task<IHttpActionResult> Register(int param_ID  )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser() {Id = Guid.NewGuid().ToString("N"), UserName = model.userName, Email = model.email  };

            IdentityResult result =  UserManager.Create(user, model.password);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }
            var groupJoin = new
            {
                userName = user.UserName,
                userId = user.Id,
                email = user.Email

            };

            return Ok(user);
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            throw new NotImplementedException();
        }

        // DELETE: api/Users/5
        [ResponseType(typeof(AspNetUsers))]
        public IHttpActionResult DeleteAspNetUsers(string id)
        {
            AspNetUsers aspNetUsers = db.AspNetUsers.Find(id);
            if (aspNetUsers == null)
            {
                return NotFound();
            }

            db.AspNetUsers.Remove(aspNetUsers);
            db.SaveChanges();

            return Ok(aspNetUsers);
        }
        [Route("api/Users/changePassword")]  
        public async Task<IHttpActionResult> ChangePassword(string Userid,string UserOldPassword ,string UserNewPassword)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.ChangePasswordAsync(Userid,UserOldPassword,
               UserNewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AspNetUsersExists(string id)
        {
            return db.AspNetUsers.Count(e => e.Id == id) > 0;
        }
    }
}