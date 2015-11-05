﻿using Akka.Configuration;
using Akka.Persistence.TestKit.Journal;
using Akka.Util.Internal;
using Xunit.Abstractions;

namespace Akka.Persistence.Sqlite.Tests
{
    public class SqliteJournalSpec : JournalSpec
    {
        private static AtomicCounter counter = new AtomicCounter(0);

        public SqliteJournalSpec(ITestOutputHelper output)
            : base(CreateSpecConfig("FullUri=file:memdb-journal-" + counter.IncrementAndGet() + ".db?mode=memory&cache=shared;"), "SqliteJournalSpec", output)
        {
            SqlitePersistence.Get(Sys);

            Initialize();
        }

        private static Config CreateSpecConfig(string connectionString)
        {
            return ConfigurationFactory.ParseString(@"
                akka.persistence {
                    publish-plugin-commands = on
                    journal {
                        plugin = ""akka.persistence.journal.sqlite""
                        sqlite {
                            class = ""Akka.Persistence.Sqlite.Journal.SqliteJournal, Akka.Persistence.Sqlite""
                            plugin-dispatcher = ""akka.actor.default-dispatcher""
                            table-name = event_journal
                            auto-initialize = on
                            connection-string = """ + connectionString + @"""
                        }
                    }
                }");
        }
    }
}