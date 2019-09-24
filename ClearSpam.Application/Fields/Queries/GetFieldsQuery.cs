using ClearSpam.Application.Models;
using MediatR;
using System.Collections.Generic;

namespace ClearSpam.Application.Fields.Queries
{
    public class GetFieldsQuery : IRequest<IEnumerable<FieldDto>>
    {
    }
}
