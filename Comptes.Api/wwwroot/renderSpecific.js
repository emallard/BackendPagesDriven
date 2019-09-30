
addRenderer(
    (x, h) => h == "HomePage.Pages.[]",
    (x, h) => `<a href="${href(x.Link)}">${x.PageName}</a>`);

addRenderer(
    (x, h) => h == "HomePage.Entities.[]",
    (x, h) => `<a href="${href(x.Link)}">${x.EntityName}</a>`);
addRenderer(
    (x, h) => h == "HomePage.Tests.[]",
    (x, h) => `<a href="${href(x.Link)}">${x.TestName}</a>`);


addRenderer(
    (x, h) => h == "PosteListPage.Postes.[]",
    (x, h) => `<a href="${href(x.Lien)}">${x.Poste.Nom}</a>`);
