using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using SharedPlaylist.Models;
using SharedPlaylistApi.Models;

namespace SharedPlaylistApi.Controllers
{
    public class CommentsController : ApiController
    {
        private CommentsModel db = new CommentsModel();

        // GET: api/Comments
        public IQueryable<Comments> GetComments()
        {
            return db.Comments.OrderBy(e => e.Order);
        }



        [HttpGet]
        [ActionName("GetCommentsForPlaylistIds")]
        public async Task<IHttpActionResult> GetCommentsForPlaylistIds([FromUri] string[] playlistIds)
        {
            var comments = new List<Comments>();

            foreach (var playlistId in playlistIds)
            {
                var comment = db.Comments.Where(e => e.PlaylistId == playlistId);

                if (comment.Any())
                {
                    comments.AddRange(comment);
                }
            }


            //foreach (var comment in playlistIds.Select(playlistId => db.Comments.Where(e => e.PlaylistId == playlistId)).Where(comment => comment.Any()))
            //{
            //comments.AddRange(comment);
            //}

            return Ok(comments);
        }


        [HttpGet]
        [ActionName("GetCommentsForTrackById")]
        public async Task<IHttpActionResult> GetCommentsForTrackById(string id)
        {
            IEnumerable<Comments> comments = db.Comments.Where(e => e.TrackId == id);
            if (!comments.Any())
            {
                return NotFound();
            }

            return Ok(comments);
        }



        // GET: api/Comments/5
        [ResponseType(typeof(Comments))]
        public async Task<IHttpActionResult> GetComments(int id)
        {
            Comments comments = await db.Comments.FindAsync(id);
            if (comments == null)
            {
                return NotFound();
            }

            return Ok(comments);
        }

        // PUT: api/Comments/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutComments(int id, Comments comments)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != comments.Id)
            {
                return BadRequest();
            }

            db.Entry(comments).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommentsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Comments
        [ResponseType(typeof(Comments))]
        public async Task<IHttpActionResult> PostComments(Comments comments)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // set next order number
            var dbComments = db.Comments.Where(e => e.PlaylistId == comments.PlaylistId && e.TrackId == comments.TrackId);
            if (dbComments.Any())
            {
                var lastOrder = dbComments.OrderByDescending(e => e.Order).First();
                var newOrder = lastOrder.Order;
                comments.Order = newOrder++;
            }


            db.Comments.Add(comments);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = comments.Id }, comments);
        }

        // DELETE: api/Comments/5
        [ResponseType(typeof(Comments))]
        public async Task<IHttpActionResult> DeleteComments(int id)
        {
            Comments comments = await db.Comments.FindAsync(id);
            if (comments == null)
            {
                return NotFound();
            }

            db.Comments.Remove(comments);
            await db.SaveChangesAsync();

            return Ok(comments);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CommentsExists(int id)
        {
            return db.Comments.Count(e => e.Id == id) > 0;
        }
    }
}