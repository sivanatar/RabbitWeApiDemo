using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RabbitWeApiDemo.Entities;
using RabbitWeApiDemo.MessageBusRepositories;

namespace RabbitWeApiDemo.Controllers
{
    [Route("api/[controller]")]
    public class AuthorController : Controller
    {
        private IEnumerable<Author> GetAllAuthors()
        {
            var authors = new List<Author>();
            Author author;
            do
            {
                author = MessageRepository.GetFirstMessage();
                if (author != null)
                {
                    authors.Add(author);
                }
            } while (author != null);
            return authors;
        }

        private void AddObjectToRepository(Author author)
        {
            var json = JsonConvert.SerializeObject(author);
            var body = Encoding.UTF8.GetBytes(json);
            MessageRepository.PostMessage(body);
        }


        public IMessageRepository<Author> MessageRepository
        {
            get
            {
                return MessageRepositoryFactory.Create(RepositoryName);
            }
        }

        public IMessageRepositoryFactory<Author> MessageRepositoryFactory { get; set; }

        public string RepositoryName { get => MessageRepositoryFactory.RepositoryName; }

        public AuthorController(IMessageRepositoryFactory<Author> repositoryFactory)
        {
            MessageRepositoryFactory = repositoryFactory;
        }

        //TODO we can abstract all Get/All/Post methods under base class
        // GET api/Author/Get
        [HttpGet]
        [Route("Get")]
        public IActionResult Get()
        {
            var author = MessageRepository.GetFirstMessage();
            if (author != null)
            {
                return Ok(author);
            }
            return NotFound();
        }

        // GET api/Author/All
        [HttpGet]
        [Route("All")]
        public IEnumerable<Author> All()
        {
            return GetAllAuthors();
        }

        // POST api/Author/Add
        [HttpPost]
        [Route("Add")]
        public IActionResult Post([FromBody]Author author)
        {
            AddObjectToRepository(author);

            return Created("/api/Author/Get", author);
        }
    }
}
