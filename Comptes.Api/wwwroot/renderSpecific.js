
addRenderer(
    (x, h, r) => (0 <= h.indexOf("HomePage.Pages.Result.[")),
    (x, h, r) => `<a href="${href(x.Link)}">${x.PageName}</a>`);

addRenderer(
    (x, h, r) => (0 <= h.indexOf("HomePage.Entities.Result.[")),
    (x, h, r) => `<a href="${href(x.Link)}">${x.EntityName}</a>`);
addRenderer(
    (x, h, r) => (0 <= h.indexOf("HomePage.Tests.Result.[")),
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
    (x, h, r) => h == "TestPageResponse.TestReport.Result.Path.Items",
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

addRenderer(
    (x, h, r) => h == 'TestPageResponse.PageGraph.Result',
    (x, h, r) => {
        r.afterRender(() => setTimeout(() =>
            drawInGraph(
                _page.TestReport.Result.Path.Items,
                document.getElementById('TestPageResponse.PageGraph.Result.Svg'))
            , 1000));
        return `${x.Svg}`.replace('<svg', `<svg id="${h + '.Svg'}"`);
    }
);

