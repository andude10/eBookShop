﻿using TheTome.Models;

namespace TheTome.ViewModels;

public class CatalogViewModel
{
    public CatalogViewModel(BooksViewModel booksViewModel, PageViewModel pageViewModel, SortBookState sortBookState)
    {
        BooksViewModel = booksViewModel;
        PageViewModel = pageViewModel;
        SortBookState = sortBookState;
    }

    public BooksViewModel BooksViewModel { get; set; }
    public PageViewModel PageViewModel { get; set; }
    public SortBookState SortBookState { get; set; }
}