﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfirmRep.Infrastructure.Common
{
    public interface IMapper
    {
        //
        // Summary:
        //     Execute a mapping from the source object to a new destination object.  The
        //     source type is inferred from the source object.
        //
        // Parameters:
        //   source:
        //     Source object to map from
        //
        // Type parameters:
        //   TDestination:
        //     Destination type to create
        //
        // Returns:
        //     Mapped destination object
        TDestination Map<TDestination>(object source);
        //
        // Summary:
        //     Execute a mapping from the source object to a new destination object.
        //
        // Parameters:
        //   source:
        //     Source object to map from
        //
        // Type parameters:
        //   TSource:
        //     Source type to use, regardless of the runtime type
        //
        //   TDestination:
        //     Destination type to create
        //
        // Returns:
        //     Mapped destination object
        TDestination Map<TSource, TDestination>(TSource source);
    }
}
