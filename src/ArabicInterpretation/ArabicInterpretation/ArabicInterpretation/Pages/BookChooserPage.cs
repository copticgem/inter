﻿using ArabicInterpretation.Model;
using ArabicInterpretation.Views;
using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ArabicInterpretation.Pages
{
    public class BookChooserPage : BasePage
    {
        private const string AuthorChangedMessage = "BookChooserPageAuthorChanged";

        Author author;
        bool isNT;
        int bookNumber;

        BookChooser bookChooser;
        AuthorLabel authorLabel;

        public BookChooserPage()
        {
            StackLayout layout = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
            };

            this.authorLabel = new AuthorLabel();
            layout.Children.Add(authorLabel);

            this.bookChooser = new BookChooser();
            layout.Children.Add(this.bookChooser);

            this.Content = layout;

            // Listen to author changes to disable missing books
            MessagingCenter.Subscribe<AuthorsGrid, Author>(this, BookChooserPage.AuthorChangedMessage, async (sender, arg) =>
            {
                if (this.author != arg)
                {
                    this.author = arg;

                    await this.Initialize(
                        author: arg,
                        isNT: this.isNT,
                        bookNumber: this.bookNumber);
                }
            });
        }

        public async Task Initialize(
            Author author,
            bool isNT,
            int bookNumber)
        {
            this.author = author;
            this.isNT = isNT;
            this.bookNumber = bookNumber;

            await this.authorLabel.Initialize(
                BookChooserPage.AuthorChangedMessage,
                author,
                isNT,
                bookNumber);

            await bookChooser.Initialize(
                author: author,
                isNT: isNT,
                currentBookNumber: bookNumber);
        }
    }
}
