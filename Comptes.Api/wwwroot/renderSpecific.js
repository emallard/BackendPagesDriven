
addRenderer(
    (x, h, r) => h == "HomePage.Pages.[]",
    (x, h, r) => `<a href="${href(x.Link)}">${x.PageName}</a>`);

addRenderer(
    (x, h, r) => h == "HomePage.Entities.[]",
    (x, h, r) => `<a href="${href(x.Link)}">${x.EntityName}</a>`);
addRenderer(
    (x, h, r) => h == "HomePage.Tests.[]",
    (x, h, r) => `<a href="${href(x.Link)}">${x.TestName}</a>`);


addRenderer(
    (x, h, r) => h == "PosteListPage.Postes.[]",
    (x, h, r) => `<a href="${href(x.Lien)}">${x.Poste.Nom}</a>`);


addRenderer(
    (x, h, r) => h == "PagePage.PageReport.RepositoryItems",
    (x, h, r) => {
        return renderArrayAsDatatable2(x, h, r,
            {
                bFilter: true,
                bPaginate: false,
                bInfo: false,
                "columns": [
                    { "data": "Operation", "title": "Operation" },
                    { "data": "EntityName", "title": "EntityName" },
                    { "data": "MessageName", "title": "MessageName" }
                ]
            })
    });

addRenderer(
    (x, h, r) => h == "EntityPage.EntityReport.RepositoryItems",
    (x, h, r) => {
        return renderArrayAsDatatable2(x, h, r,
            {
                bFilter: true,
                bPaginate: false,
                bInfo: false,
                //"data": x,
                "columns": [
                    { "data": "Operation", "title": "Operation" },
                    { "data": "MessageName", "title": "MessageName" },
                    { "data": "PageName", "title": "PageName" }
                ]
            })
    });

addRenderer(
    (x, h, r) => h == "TestPageResponse.TestReport.Path.Items",
    (x, h, r) => {
        return renderArrayAsTable(x, h, r,
            {
                "columns": [

                    { "data": "IndexInTest", "title": "#" },
                    { "data": "PageFrom", "title": "From" },
                    { "data": "PageFromMemberName", "title": "Member" },
                    { "data": "PageTo", "title": "To" },
                    { "data": "PageToHasAssert", "title": "Assert" }
                ]
            })
    });

