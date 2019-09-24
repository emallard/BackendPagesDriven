using System;
using Xunit;
using Ninject;
using Ninject.Extensions.ContextPreservation;
using Ninject.Extensions.NamedScope;
using System.Threading.Tasks;
using FluentAssertions;

namespace CocoriCore.Mapper.Comptes
{
    public class TestComptes1 : TestBase
    {
        [Fact]
        public async void Test1()
        {
            var id = await this.ExecuteAsync(new CreateCommand<PosteCreate>()
            {
                Object = new PosteCreate()
                {
                    Nom = "Voiture"
                }
            });

            var posteView = await this.ExecuteAsync(new ByIdQuery<PosteView>()
            {
                Id = id
            });
            posteView.Id.Id.Should().Be(id);
            posteView.Nom.Should().Be("Voiture");


            await this.ExecuteAsync(new UpdateCommand<PosteUpdate>()
            {
                Object = new PosteUpdate()
                {
                    Id = new TypedId<Poste>() { Id = id },
                    Nom = "Bijoux"
                }
            });

            posteView = await this.ExecuteAsync(new ByIdQuery<PosteView>()
            {
                Id = id
            });
            posteView.Id.Id.Should().Be(id);
            posteView.Nom.Should().Be("Bijoux");
        }
    }
}
