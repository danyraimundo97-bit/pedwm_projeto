using System;
using DomainLayer.Domain;

namespace DomainLayer.Builders
{
    // O <TBuilder> garante que devolvemos sempre a classe final (o filho)
    public abstract class ProjectBaseBuilder<TBuilder> where TBuilder : ProjectBaseBuilder<TBuilder>
    {
        protected string _title = string.Empty;
        protected DateTime _startDate;
        protected DateTime _endDate;

        public TBuilder WithTitle(string title)
        {
            _title = title;
            return (TBuilder)this; // Fazemos "cast" para o tipo do filho
        }

        public TBuilder WithDates(DateTime start, DateTime end)
        {
            _startDate = start;
            _endDate = end;
            return (TBuilder)this;
        }
    }
}