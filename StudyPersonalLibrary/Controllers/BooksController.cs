using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudyPersonalLibrary.Data;
using StudyPersonalLibrary.Models;
using Microsoft.Data.SqlClient;

namespace StudyPersonalLibrary.Controllers
{
    public class BooksController : Controller
    {
        //private readonly StudyPersonalLibraryContext _context;
        private readonly IConfiguration _config;
        private string ConnectionString;

        public BooksController(IConfiguration configuration)
        {
            _config = configuration;
            ConnectionString = _config["ConnectionStrings:DevConnection"];
        }

        // GET: Books
        //public async Task<IActionResult> Index()
        //{
        //    DataTable dtbl = new DataTable();
        //    using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
        //    {
        //        sqlConnection.Open();
        //        SqlDataAdapter sqlDa = new SqlDataAdapter("BookViewAll" , sqlConnection);
        //        sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
        //        sqlDa.Fill(dtbl);
        //    }
        //    return View(dtbl);
        //}
        public IActionResult Index()
        {
            DataTable dtbl = new DataTable();
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                sqlConnection.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("BookViewAll", sqlConnection);
                sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDa.Fill(dtbl);
            }
            return View(dtbl);
        }

        public IActionResult AddOrEdit(int? id)
        {
            Book book = new Book();
            if (id > 0)
                book = FetchBookByID(id);
            return View(book);
        }

        [HttpPost]
        public IActionResult AddOrEdit(int id, [Bind("BookID, Title, Author, Price")] Book book)
        {
            if(ModelState.IsValid)
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
                {
                    sqlConnection.Open();
                    SqlCommand sqlCmd = new SqlCommand("BookAddOrEdit", sqlConnection);
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.AddWithValue("BookID", book.BookID);
                    sqlCmd.Parameters.AddWithValue("Title", book.Title);
                    sqlCmd.Parameters.AddWithValue("Author", book.Author);
                    sqlCmd.Parameters.AddWithValue("Price", book.Price);
                    sqlCmd.ExecuteNonQuery();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        public IActionResult Delete(int? id)
        {
            Book book = new Book();
            if (id > 0)
                book = FetchBookByID(id);

            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                sqlConnection.Open();
                SqlCommand sqlCmd = new SqlCommand("DeleteBook", sqlConnection);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("BookID", book.BookID);
                sqlCmd.ExecuteNonQuery();
            }
            return RedirectToAction(nameof(Index));

            return View(book);
        }

        public Book FetchBookByID(int? id)
        {
            Book book = new Book();
            DataTable dtbl = new DataTable();
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                sqlConnection.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("BookViewByID" , sqlConnection);
                sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDa.SelectCommand.Parameters.AddWithValue("BookID", id);
                sqlDa.Fill(dtbl);
                if (dtbl.Rows.Count == 1)
                {
                    book.BookID = Convert.ToInt32(dtbl.Rows[0]["BookID"].ToString());
                    book.Title = dtbl.Rows[0]["Title"].ToString();
                    book.Author = dtbl.Rows[0]["Author"].ToString();
                    book.Price = Convert.ToInt32(dtbl.Rows[0]["Price"].ToString());
                }
                return book;
            }
        }

    }
}
