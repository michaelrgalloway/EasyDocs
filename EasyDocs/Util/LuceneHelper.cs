using EasyDocs.ApiModels;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Search.Highlight;
using Lucene.Net.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace EasyDocs.Util
{
    // This class is borrowed from the LuceneNetTutorial project with some minor changes
    public class LuceneHelper
    {
        public string IndexDirectory
        {
            get
            {
                return HttpContext.Current.Server.MapPath("~/SearchIndex");
            }
        }
        public void PopulateIndex(List<IndexStore> objectList)
        {
            using (var writer = new IndexWriter(FSDirectory.Open(IndexDirectory), new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30), true, IndexWriter.MaxFieldLength.LIMITED))
            {
                writer.UseCompoundFile = true;
                foreach (var o in objectList)
                {
                    Document doc = new Document();

                    doc.Add(new Field("text", ParseHtml(o.Content ?? ""), Field.Store.YES, Field.Index.ANALYZED));
                    doc.Add(new Field("type", o.Type, Field.Store.YES, Field.Index.NOT_ANALYZED));
                    doc.Add(new Field("searchterms", o.SearchTerms ?? "", Field.Store.YES, Field.Index.ANALYZED));
                    doc.Add(new Field("title", o.Title ?? "", Field.Store.YES, Field.Index.ANALYZED));
                    doc.Add(new Field("urlkey", o.UrlKey ?? "", Field.Store.YES, Field.Index.ANALYZED));

                    writer.AddDocument(doc);
                }

            }
        }

        public List<IndexResult> Search(string terms)
        {
            List<IndexResult> retObj = new List<IndexResult>();
            var analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);

            using (var searcher = new IndexSearcher(FSDirectory.Open(IndexDirectory)))
            {

                // parse the query, "text" is the default field to search
                var parser = new MultiFieldQueryParser(Lucene.Net.Util.Version.LUCENE_30, new[] { "text", "title", "urlkey", "searchterms" }, analyzer);

                Query query = parser.Parse(terms);

                TopDocs hits = searcher.Search(query, 200);

                
                SimpleFragmenter fragmenter = new SimpleFragmenter(80);
                QueryScorer scorer = new QueryScorer(query);
                Highlighter highlighter = new Highlighter(scorer);
                highlighter.TextFragmenter = fragmenter;

                for (int i = 0; i < hits.TotalHits; i++)
                {
                    // get the document from index
                    Document doc = searcher.Doc(hits.ScoreDocs[i].Doc);

                    TokenStream stream = analyzer.TokenStream("", new StringReader(doc.Get("text")));

                    String sample = highlighter.GetBestFragments(stream, doc.Get("text"), 2, "...");
                    String title = doc.Get("title");
                    String urlkey = doc.Get("urlkey");
                    String type = doc.Get("type");

                    retObj.Add(new IndexResult()
                    {
                        Sample = sample,
                        Title = title,
                        Type = type,
                        UrlKey = urlkey

                    });

                }

                return retObj;
            }
        }

        private static string ParseHtml(string html)
        {
            string temp = Regex.Replace(html, "<[^>]*>", "");
            return temp.Replace("&nbsp;", " ");
        }
    }
}