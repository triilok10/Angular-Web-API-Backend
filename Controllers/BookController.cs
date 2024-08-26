using Angular_Web_API.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace Angular_Web_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        #region "Constructor / Calling _connectionString"

        private readonly string _connectionString;
        public BookController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("CustomConnection");
        }
        #endregion

        #region "Insert Book"
        [HttpPost]
        public IActionResult InsertBook(Book pBook)
        {
            string Message = "";
            bool Response = false;
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("usp_Books", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Mode", 1);
                    cmd.Parameters.AddWithValue("@BookName", pBook.BookName);
                    cmd.Parameters.AddWithValue("@BookAuthorName", pBook.BookAuthorName);
                    cmd.Parameters.AddWithValue("@BookMedium", pBook.BookMedium);
                    cmd.Parameters.AddWithValue("@BookPublishedYear", pBook.BookPublishedYear);
                    cmd.ExecuteNonQuery();
                    Message = "Book added successfully";
                    Response = true;
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Response = false;
            }
            return Ok(new { res = Response, msg = Message });
        }

        #endregion


        #region "Get Book List"
        [HttpGet]
        public IActionResult GetBookList()
        {
            string Message = "";
            bool Response = false;
            try
            {
                List<Book> lstBook = new List<Book>();

                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("usp_Books", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Mode", 2);

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            Book obj = new Book
                            {
                                BookID = Convert.ToInt32(rdr["BookID"]),
                                BookName = Convert.ToString(rdr["BookName"]),
                                BookAuthorName = Convert.ToString(rdr["BookAuthorName"]),
                                BookMedium = Convert.ToString(rdr["BookMedium"]),
                                BookPublishedYear = Convert.ToString(rdr["BookPublishedYear"])
                            };
                            lstBook.Add(obj);
                        }
                    }
                }
                Message = "Book List retrived successfully";
                Response = true;
                return Ok(new { getList = lstBook });
            }
            catch (Exception ex)
            {
                Message = "Error in adding the Book";
                Response = false;
            }
            return Ok(new
            {
                res = Response,
                msg = Message
            });
        }

        #endregion

        #region "Get Book by Id"
        [HttpGet]
        public IActionResult GetBook(int bookId = 0)
        {
            string Message = "";
            bool response = false;
            try
            {
                if (bookId <= 0)
                {
                    Message = "Please select the Book to update";
                    response = false;
                    return Ok(new { msg = Message, res = response });
                }

                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("usp_Books", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Mode", 3);
                    cmd.Parameters.AddWithValue("@BookId", bookId);

                    Book obj = new Book();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            obj.BookID = Convert.ToInt32(rdr["BookID"]);
                            obj.BookName = Convert.ToString(rdr["BookName"]);
                            obj.BookAuthorName = Convert.ToString(rdr["BookAuthorName"]);
                            obj.BookMedium = Convert.ToString(rdr["BookMedium"]);
                            obj.BookPublishedYear = Convert.ToString(rdr["BookPublishedYear"]);
                        }
                    }
                    return Ok(obj);
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                response = false;
                return Ok(new { msg = Message, res = response });
            }

        }
        #endregion

        #region "Get Book by Id"
        [HttpDelete("{bookID}")]
        public IActionResult DeleteBook(int bookID = 0)
        {
            string Message = "";
            bool response = false;
            try
            {
                if (bookID <= 0)
                {
                    Message = "Please select the Book to Delete";
                    response = false;
                    return Ok(new { msg = Message, res = response });
                }

                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("usp_Books", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Mode", 5);
                    cmd.Parameters.AddWithValue("@BookId", bookID);
                    cmd.ExecuteNonQuery();
                    Message = "Book deleted successfully";
                    response = true;
                    return Ok(new { msg = Message, res = response });
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                response = false;
                return Ok(new { msg = Message, res = response });
            }

        }
        #endregion

    }
}
