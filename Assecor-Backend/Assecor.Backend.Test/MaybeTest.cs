using Assecor.Backend.Domain.ApiModels;
using Assecor.Backend.Domain.BackendModels;
using Assecor.Backend.Domain.Maybe;

namespace Assecor.Backend.Test
{
    public class MaybeTest
    {
        [Fact]
        public void Should_Have_Value()
        {
            var value = 1;
            var maybe = Maybe.From(value);

            maybe.HasValue.ShouldBeTrue();
            maybe.Value.ShouldBe(value);
        }

        [Fact]
        public void Should_Have_No_Value()
        {
            int? value = null;
            var maybe = Maybe.From(value);

            maybe.HasValue.ShouldBeFalse();
            var act = () => maybe.Value.ShouldBe(value);
            act.ShouldThrow<InvalidOperationException>("Maybe has no value");
        }

        [Fact]
        public void Should_Map_Correctly()
        {
            var maybe = Maybe.From(new Person());

            var mapped = maybe.Map(_ => new ApiPerson());

            mapped.GetTypeName().ShouldBe(nameof(ApiPerson));
        }

        [Fact]
        public void Should_Throw_On_Missing_Mapping_Func()
        {
            Func<Person, ApiPerson> mappingFunc = null!;
            var maybe = Maybe.From(new Person());
            Func<object?> func = () => maybe.Map(mappingFunc!);

            func.ShouldThrow<ArgumentNullException>($"{nameof(mappingFunc)}");
        }
    }
}
