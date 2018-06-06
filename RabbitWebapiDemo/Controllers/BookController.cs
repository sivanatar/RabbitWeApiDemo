using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RabbitWeApiDemo.Entities;
using RabbitWeApiDemo.MessageBusRepositories;

namespace RabbitWeApiDemo.Controllers
{
    [Route("api/[controller]")]
    public class BookController : Controller
    {
        private void AddObjectToRepository(Book book)
        {
            var json = JsonConvert.SerializeObject(book);
            var body = Encoding.UTF8.GetBytes(json);
            MessageRepository.PostMessage(body);
        }

        private IEnumerable<Book> GetAllBooks()
        {
            var books = new List<Book>();
            Book book;
            do
            {
                book = MessageRepository.GetFirstMessage();
                if (book != null)
                {
                    books.Add(book);
                }
            } while (book != null);
            return books;
        }

        public IMessageRepository<Book> MessageRepository
        {
            get
            {
                return MessageRepositoryFactory.Create(RepositoryName);
            }
        }

        public IMessageRepositoryFactory<Book> MessageRepositoryFactory { get; set; }

        public string RepositoryName { get => MessageRepositoryFactory.RepositoryName; }

        public BookController(IMessageRepositoryFactory<Book> repositoryFactory)
        {
            MessageRepositoryFactory = repositoryFactory;
        }

        //TODO we can abstract all Get/All/Post methods under base class

        // GET api/Book/Get
        [HttpGet]
        [Route("Get")]
        public IActionResult Get()
        {
            var book = MessageRepository.GetFirstMessage();
            if (book != null)
            {
                return Ok(book);
            }
            return NotFound();
        }

        // GET api/Book/All
        [HttpGet]
        [Route("All")]
        public IEnumerable<Book> All()
        {
            return GetAllBooks();
        }

        // POST api/Book/Add
        [HttpPost]
        [Route("Add")]
        public IActionResult Post([FromBody]Book book)
        {
            AddObjectToRepository(book);
            return Created("/api/Book/Get", book);
        }
    }
}
