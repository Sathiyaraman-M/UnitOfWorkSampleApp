using Microsoft.EntityFrameworkCore;
using UnitOfWorkSampleApp;
using UnitOfWorkSampleApp.Repositories;

Console.WriteLine("Hello World - Test Application");
Console.WriteLine("-----------------------------------------------");

Console.WriteLine("Creating an UnitOfWork instance....\n");
UnitOfWork<int> _unitOfWork = new UnitOfWork<int>(new LibraryDbContext());

Console.WriteLine("Adding Books....");
if (!await _unitOfWork.Repository<Book>().Entities.AnyAsync())
{
    Book book1 = new()
    {
        Name = "Jurassic Park",
        ISBN = "5423809894742",
        DeweyIndex = "731.478-BHJ",
        Author = "Micheal Crichton",
        Cost = 500M,
        Edition = "Second",
        Publisher = "Gilbert Books",
        PublicationYear = 2016,
        Description = "Best science fiction novel about possibility of bringing dinosaurs back"
    };

    Book book2 = new()
    {
        Name = "Dune",
        ISBN = "1564232894742",
        DeweyIndex = "741.821-NMV",
        Author = "Frank Herbert",
        Cost = 800M,
        Edition = "Third",
        Publisher = "Alphine Publications",
        PublicationYear = 2018,
        Description = "Best science fiction novel about future human civilizations across interstellar space"
    };

    await _unitOfWork.Repository<Book>().AddAsync(book1);
    await _unitOfWork.Repository<Book>().AddAsync(book2);
    await _unitOfWork.Commit(new CancellationToken());
}
Console.WriteLine("Added books successfully\n");

Console.WriteLine("Adding Book headers....");
if(!await _unitOfWork.Repository<BookHeader>().Entities.AnyAsync())
{
    BookHeader bookHeader1 = new()
    {
        Barcode = "76814",
        IsAvailable = true,
        Condition = "Good Condition",
        Book = await _unitOfWork.Repository<Book>().GetByIdAsync(1)
    };

    BookHeader bookHeader2 = new()
    {
        Barcode = "23521",
        IsAvailable = false,
        Condition = "Good Condition",
        Book = await _unitOfWork.Repository<Book>().GetByIdAsync(1)
    };

    BookHeader bookHeader3 = new BookHeader()
    {
        Barcode = "54325",
        IsAvailable = true,
        Condition = "Damaged",
        Book = await _unitOfWork.Repository<Book>().GetByIdAsync(2)
    };

    await _unitOfWork.Repository<BookHeader>().AddAsync(bookHeader1);
    await _unitOfWork.Repository<BookHeader>().AddAsync(bookHeader2);
    await _unitOfWork.Repository<BookHeader>().AddAsync(bookHeader3);
    await _unitOfWork.Commit(new CancellationToken());
}
Console.WriteLine("Added Book headers successfully\n");

Console.WriteLine("Updating books with book header count details....");
var books = await _unitOfWork.Repository<Book>().GetAllAsync();
foreach (var book in books)
{
    book.Copies = await _unitOfWork.Repository<BookHeader>().Entities.CountAsync(x => x.BookId == book.Id);
    book.AvailableCount = await _unitOfWork.Repository<BookHeader>().Entities.CountAsync(x => x.BookId == book.Id && x.Condition == "Good Condition");
    book.DamagedCount = await _unitOfWork.Repository<BookHeader>().Entities.CountAsync(x => x.BookId == book.Id && x.Condition == "Damaged");
    book.MissingCount = await _unitOfWork.Repository<BookHeader>().Entities.CountAsync(x => x.BookId == book.Id && x.Condition == "Missing");
    await _unitOfWork.Repository<Book>().UpdateAsync(book);
    await _unitOfWork.Commit(new CancellationToken());
}
Console.WriteLine("Updated successfully\n");

Console.WriteLine("Application ends here!");