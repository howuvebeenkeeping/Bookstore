using Moq;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Xunit;

namespace Bookstore.Tests {
    public class BookServiceTests {
        [Fact]
        public void GetAllByQuery_WithIsbn_CallsGetAllByIsbn() {
            var bookRepositoryStub = new Mock<IBookRepository>();

            bookRepositoryStub.Setup(bookRepository => bookRepository.GetAllByIsbn(It.IsAny<string>()))
                              .Returns(new[] { new Book(1, "", "", "", "", 0m) });

            bookRepositoryStub.Setup(bookRepository => bookRepository.GetAllByTitleOrAuthor(It.IsAny<string>()))
                              .Returns(new[] { new Book(2, "", "", "", "", 0m) });

            var bookService = new BookService(bookRepositoryStub.Object);

            var validIsbn = "ISBN 12345-67890";
            Book[] books = bookService.GetAllByQuery(validIsbn);

            Assert.Collection(books, book => Assert.Equal(1, book.Id));
        }

        [Fact]
        public void GetAllByQuery_WithAuthor_CallsGetAllByTitleOrAuthor() {
            var bookRepositoryStub = new Mock<IBookRepository>();

            bookRepositoryStub.Setup(x => x.GetAllByIsbn(It.IsAny<string>()))
                              .Returns(new[] { new Book(1, "", "", "", "", 0m) });

            bookRepositoryStub.Setup(x => x.GetAllByTitleOrAuthor(It.IsAny<string>()))
                              .Returns(new[] { new Book(2, "", "", "", "", 0m) });

            var bookService = new BookService(bookRepositoryStub.Object);
            
            var invalidIsbn = "12345-56789";
            Book[] books = bookService.GetAllByQuery(invalidIsbn);

            Assert.Collection(books, book => Assert.Equal(2, book.Id));
        }

        // without Moq
        [Fact]
        public void GetAllByQuery_WithIsbn_CallsGetAllByIsbn_WithoutMock() {
            var stubBookRepository = new StubBookRepository {
                ResultOfGetAllByIsbn = new[] {
                    new Book(1, "", "", "", "", 0m)
                },
                ResultOfGetAllByTitleOrAuthor = new[] {
                    new Book(2, "", "", "", "", 0m)
                }
            };

            var bookService = new BookService(stubBookRepository);

            Book[] books = bookService.GetAllByQuery("ISBN 12345-67890");

            Assert.Collection(books, book => Assert.Equal(1, book.Id));

        }
        
        [Fact]
        public void GetAllByQuery_WithIsbn_CallsGetAllByTitleOrAuthor_WithoutMock() {
            var stubBookRepository = new StubBookRepository {
                ResultOfGetAllByIsbn = new[] {
                    new Book(1, "", "", "", "", 0m)
                },
                ResultOfGetAllByTitleOrAuthor = new[] {
                    new Book(2, "", "", "", "", 0m)
                }
            };

            var bookServices = new BookService(stubBookRepository);

            Book[] books = bookServices.GetAllByQuery("12345 67890");
            
            Assert.Collection(books, book => Assert.Equal(2, book.Id));
        }
    }
}
