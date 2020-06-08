using System;
using System.Collections.Generic;
using System.Data;
using JetBrains.Annotations;
using NUnit.Framework;
using QuikGraph.Graphviz.Dot;

namespace QuikGraph.Data.Tests
{
    /// <summary>
    /// Tests related to <see cref="DataSetGraphvizAlgorithm"/>.
    /// </summary>
    [TestFixture]
    internal class DataSetGraphvizAlgorithmTests
    {
        [Test]
        public void Constructor()
        {
            DataSetGraph graph = new DataSet().ToGraph();
            DataSetGraph otherGraph = new DataSet().ToGraph();
            var algorithm = new DataSetGraphvizAlgorithm(graph);
            AssertAlgorithmProperties(algorithm, graph);

            algorithm = new DataSetGraphvizAlgorithm(graph, GraphvizImageType.Fig);
            AssertAlgorithmProperties(algorithm, graph, GraphvizImageType.Fig);

            algorithm = new DataSetGraphvizAlgorithm(graph, GraphvizImageType.Ps);
            AssertAlgorithmProperties(algorithm, graph, GraphvizImageType.Ps);

            algorithm = new DataSetGraphvizAlgorithm(graph, GraphvizImageType.Hpgl);
            algorithm.ImageType = GraphvizImageType.Gd;
            AssertAlgorithmProperties(algorithm, graph, GraphvizImageType.Gd);

            algorithm = new DataSetGraphvizAlgorithm(graph);
            algorithm.VisitedGraph = otherGraph;
            AssertAlgorithmProperties(algorithm, otherGraph);

            #region Local function

            void AssertAlgorithmProperties(
                DataSetGraphvizAlgorithm algo,
                DataSetGraph treatedGraph,
                GraphvizImageType imageType = GraphvizImageType.Png)
            {
                Assert.AreSame(treatedGraph, algo.VisitedGraph);
                Assert.IsNotNull(algo.GraphFormat);
                Assert.IsNotNull(algo.CommonVertexFormat);
                Assert.IsNotNull(algo.CommonEdgeFormat);
                Assert.AreEqual(imageType, algo.ImageType);
                Assert.IsNull(algo.Output);
            }

            #endregion
        }

        [Test]
        public void Constructor_Throws()
        {
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => new DataSetGraphvizAlgorithm(null));
            Assert.Throws<ArgumentNullException>(() => new DataSetGraphvizAlgorithm(null, GraphvizImageType.Gif));

            DataSetGraph graph = new DataSet().ToGraph();
            var algorithm = new DataSetGraphvizAlgorithm(graph);
            Assert.Throws<ArgumentNullException>(() => algorithm.VisitedGraph = null);
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }

        [NotNull, ItemNotNull]
        private static IEnumerable<TestCaseData> GenerateTestCases
        {
            [UsedImplicitly]
            get
            {
                // Empty data set
                yield return new TestCaseData(new DataSet())
                {
                    ExpectedResult =
                        @"digraph G {" + Environment.NewLine
                        + @"node [shape=record, style=solid, label=""""];" + Environment.NewLine
                        + @"}"
                };

                // Only vertices
                var shopDataSet = new DataSet();

                var addresses = new DataTable("Addresses");
                var addressIdCol = new DataColumn("AddressID", typeof(int)) { Unique = true };
                addresses.Columns.Add(addressIdCol);
                shopDataSet.Tables.Add(addresses);

                var customers = new DataTable("Customers");
                var customerIdCol = new DataColumn("CustomerID", typeof(int));
                customers.Columns.Add(customerIdCol);
                shopDataSet.Tables.Add(customers);

                var orders = new DataTable("Orders");
                shopDataSet.Tables.Add(orders);

                yield return new TestCaseData(shopDataSet)
                {
                    ExpectedResult =
                        @"digraph G {" + Environment.NewLine
                        + @"node [shape=record, style=solid, label=""""];" + Environment.NewLine
                        + @"0 [shape=record, label=""Addresses | +\ AddressID\ :\ Int32\ unique""];" + Environment.NewLine
                        + @"1 [shape=record, label=""Customers | +\ CustomerID\ :\ Int32""];" + Environment.NewLine
                        + @"2 [shape=record, label=""Orders | ""];" + Environment.NewLine
                        + @"}"
                };

                // With relations
                var forumDataSet = new DataSet();

                var tmps = new DataTable("Tmps");
                forumDataSet.Tables.Add(tmps);

                var rewards = new DataTable("Rewards");
                var rewardId = new DataColumn("reward_id", typeof(int)) { Unique = true };
                rewards.Columns.Add(rewardId);
                forumDataSet.Tables.Add(rewards);

                var categories = new DataTable("Categories");
                var categoryId = new DataColumn("cat_id", typeof(int)) { Unique = true };
                var categoryName = new DataColumn("cat_name", typeof(string));
                var categoryDescription = new DataColumn("cat_description", typeof(string));
                categories.Columns.Add(categoryId);
                categories.Columns.Add(categoryName);
                categories.Columns.Add(categoryDescription);
                forumDataSet.Tables.Add(categories);

                var users = new DataTable("Users");
                var userId = new DataColumn("user_id", typeof(int)) { Unique = true };
                var userName = new DataColumn("user_name", typeof(string));
                var userPass = new DataColumn("user_pass", typeof(string));
                var userEmail = new DataColumn("user_email", typeof(string));
                var userDate = new DataColumn("user_date", typeof(DateTime));
                users.Columns.Add(userId);
                users.Columns.Add(userName);
                users.Columns.Add(userPass);
                users.Columns.Add(userEmail);
                users.Columns.Add(userDate);
                forumDataSet.Tables.Add(users);

                var replies = new DataTable("Replies");
                var replyId = new DataColumn("reply_id", typeof(int)) { Unique = true };
                var replyContent = new DataColumn("reply_content", typeof(string));
                var replyDate = new DataColumn("reply_date", typeof(DateTime));
                var replyTopic = new DataColumn("reply_topic", typeof(int));
                var replyBy = new DataColumn("reply_by", typeof(int));
                replies.Columns.Add(replyId);
                replies.Columns.Add(replyContent);
                replies.Columns.Add(replyDate);
                replies.Columns.Add(replyTopic);
                replies.Columns.Add(replyBy);
                forumDataSet.Tables.Add(replies);

                var topics = new DataTable("Topics");
                var topicId = new DataColumn("topic_id", typeof(int)) { Unique = true };
                var topicSubject = new DataColumn("topic_subject", typeof(string));
                var topicDate = new DataColumn("topic_date", typeof(DateTime));
                var topicCat = new DataColumn("topic_cat", typeof(int));
                var topicBy = new DataColumn("topic_by", typeof(int));
                topics.Columns.Add(topicId);
                topics.Columns.Add(topicSubject);
                topics.Columns.Add(topicDate);
                topics.Columns.Add(topicCat);
                topics.Columns.Add(topicBy);
                forumDataSet.Tables.Add(topics);

                forumDataSet.Relations.Add(new DataRelation("CategoriesToTopics", categoryId, topicCat));
                forumDataSet.Relations.Add(new DataRelation("UsersToTopics", userId, topicBy));
                forumDataSet.Relations.Add(new DataRelation("UsersToReplies", userId, replyBy));
                forumDataSet.Relations.Add(new DataRelation("TopicsToReplies", topicId, replyTopic));
                
                yield return new TestCaseData(forumDataSet)
                {
                    ExpectedResult =
                        @"digraph G {" + Environment.NewLine
                        + @"node [shape=record, style=solid, label=""""];" + Environment.NewLine
                        + @"0 [shape=record, label=""Tmps | ""];" + Environment.NewLine
                        + @"1 [shape=record, label=""Rewards | +\ reward_id\ :\ Int32\ unique""];" + Environment.NewLine
                        + @"2 [shape=record, label=""Categories | +\ cat_id\ :\ Int32\ unique\n+\ cat_name\ :\ String\n+\ cat_description\ :\ String""];" + Environment.NewLine
                        + @"3 [shape=record, label=""Users | +\ user_id\ :\ Int32\ unique\n+\ user_name\ :\ String\n+\ user_pass\ :\ String\n+\ user_email\ :\ String\n+\ user_date\ :\ DateTime""];" + Environment.NewLine
                        + @"4 [shape=record, label=""Replies | +\ reply_id\ :\ Int32\ unique\n+\ reply_content\ :\ String\n+\ reply_date\ :\ DateTime\n+\ reply_topic\ :\ Int32\n+\ reply_by\ :\ Int32""];" + Environment.NewLine
                        + @"5 [shape=record, label=""Topics | +\ topic_id\ :\ Int32\ unique\n+\ topic_subject\ :\ String\n+\ topic_date\ :\ DateTime\n+\ topic_cat\ :\ Int32\n+\ topic_by\ :\ Int32""];" + Environment.NewLine
                        + @"2 -> 5 [label=""CategoriesToTopics""];" + Environment.NewLine
                        + @"3 -> 5 [label=""UsersToTopics""];" + Environment.NewLine
                        + @"3 -> 4 [label=""UsersToReplies""];" + Environment.NewLine
                        + @"5 -> 4 [label=""TopicsToReplies""];" + Environment.NewLine
                        + @"}"
                };
            }
        }

        [TestCaseSource(nameof(GenerateTestCases))]
        public string Generate([NotNull] DataSet dataSet)
        {
            var algorithm = new DataSetGraphvizAlgorithm(dataSet.ToGraph());
            return algorithm.Generate();
        }

        [Test]
        public void Generate_Throws()
        {
            var dotEngine = new TestDotEngine();
            DataSetGraph graph = new DataSet().ToGraph();
            var algorithm = new DataSetGraphvizAlgorithm(graph);
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => algorithm.Generate(null, "NotSaved.dot"));
            Assert.Throws<ArgumentException>(() => algorithm.Generate(dotEngine, null));
            Assert.Throws<ArgumentException>(() => algorithm.Generate(dotEngine, string.Empty));
            Assert.Throws<ArgumentNullException>(() => algorithm.Generate(null, null));
            Assert.Throws<ArgumentNullException>(() => algorithm.Generate(null, string.Empty));
            // ReSharper restore AssignNullToNotNullAttribute
        }
    }
}