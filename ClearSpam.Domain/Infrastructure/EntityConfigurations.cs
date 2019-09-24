using ClearSpam.Domain.Interfaces;
using System;

namespace ClearSpam.Domain.Infrastructure
{
    public abstract class EntityConfigurations : IEntityConfigurations
    {
        public Type Type { get; }

        protected EntityConfigurations(Type type)
        {
            Type = type;
        }
    }
}