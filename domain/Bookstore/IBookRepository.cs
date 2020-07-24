using System;
using System.Collections.Generic;
using System.Text;

namespace Bookstore
{
	public interface IBookRepository
	{
		Book[] GetAllByTitle(string titlePart);
	}
}