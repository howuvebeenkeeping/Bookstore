using System.Collections.Generic;

namespace Bookstore {
	public interface IBookRepository {
		Book[] GetAllByIsbn(string isbn);
		Book[] GetAllByTitleOrAuthor(string titleOtAuthor);
        Book GetById(int id);
        Book[] GetAllByIds(IEnumerable<int> ids);
	}
}