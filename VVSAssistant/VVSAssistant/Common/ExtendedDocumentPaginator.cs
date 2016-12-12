using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace VVSAssistant.Common
{
    public class ExtendedDocumentPaginator : DocumentPaginator
    {
        private readonly Size _pageSize;
        private Typeface _typeface;
        private Size _margin;
        private readonly DocumentPaginator _paginator;



        public ExtendedDocumentPaginator(DocumentPaginator paginator, Size margin, Size pageSize)
        {

            _margin = margin;

            _paginator = paginator;

            _pageSize = pageSize;

            _paginator.PageSize = new Size(_pageSize.Width - margin.Width*2, _pageSize.Height - margin.Height*2);
        }



        Rect Move(Rect rect)

        {

            if (rect.IsEmpty)

            {

                return rect;

            }

            else

            {

                return new Rect(rect.Left + _margin.Width, rect.Top + _margin.Height, rect.Width, rect.Height);
            }

        }



        public override DocumentPage GetPage(int pageNumber)

        {

            var page = _paginator.GetPage(pageNumber);



            // Create a wrapper visual for transformation and add extras
            var newpage = new ContainerVisual();
            
            var smallerPage = new ContainerVisual();
            
            smallerPage.Children.Add(page.Visual);
            smallerPage.Children.Add(page.Visual);

            smallerPage.Transform = new MatrixTransform(0.95, 0, 0, 0.95, 0.025*page.ContentBox.Width, 0.025*page.ContentBox.Height);
            
            newpage.Children.Add(smallerPage);
            
            newpage.Transform = new TranslateTransform(_margin.Width, _margin.Height);

            
            return new DocumentPage(newpage, _pageSize, Move(page.BleedBox), Move(page.ContentBox));

        }



        public override bool IsPageCountValid => _paginator.IsPageCountValid;


        public override int PageCount => _paginator.PageCount;


        public override Size PageSize

        {

            get { return _paginator.PageSize; }



            set { _paginator.PageSize = value; }

        }



        public override IDocumentPaginatorSource Source => _paginator.Source;
    }
}
