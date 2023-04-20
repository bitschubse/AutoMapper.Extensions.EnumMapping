﻿using System;
using AutoMapper.Extensions.EnumMapping.Tests.Internal;
using Shouldly;
using Xunit;

namespace AutoMapper.Extensions.EnumMapping.Tests;

public class EnumValueMappingToException
{
    public class MapToExceptionNotAffectOtherMappings : AutoMapperSpecBase
    {
        Destination _result;
        public enum Source { A, B}
        public enum Destination { A = 2 }

        protected override MapperConfiguration Configuration { get; } = new MapperConfiguration(cfg =>
        {
            cfg.EnableEnumMappingValidation();
            cfg.CreateMap<Source, Destination>()
                .ConvertUsingEnumMapping(opt => opt.MapByName()
                    .MapValue(Source.A, Destination.A)
                    .MapException(Source.B, () => new NotSupportedException($"B is not valid value"))
                );
        });

        protected override void Because_of()
        {
            _result = Mapper.Map<Source, Destination>(Source.A);
        }

        [Fact]
        public void Should_map_using_custom_map()
        {
            _result.ShouldBe(Destination.A);
        }
    }
    
    public class MapToExceptionThrowException : AutoMapperSpecBase
    {
        Exception _result;
        public enum Source { A, B}
        public enum Destination { A = 2 }

        protected override MapperConfiguration Configuration { get; } = new MapperConfiguration(cfg =>
        {
            cfg.EnableEnumMappingValidation();
            cfg.CreateMap<Source, Destination>()
                .ConvertUsingEnumMapping(opt => opt.MapByName()
                    .MapValue(Source.A, Destination.A)
                    .MapException(Source.B, () => new NotSupportedException($"B is not valid value"))
                );
        });

        protected override void Because_of()
        {
            try
            {
                Mapper.Map<Source, Destination>(Source.B);
            }
            catch (Exception ex)
            {
                _result = ex;
            }
        }

        [Fact]
        public void Should_map_using_custom_map()
        {
            _result.ShouldBeOfType<NotSupportedException>();
        }
    }
}