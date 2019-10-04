_renderers.push(
    (x, h, r) => h.startsWith("PosteListPage.Postes.Result["),
    (x, h, r) => `<a id="${h}.Lien" href="${href(x.Lien)}">${x.Poste.Nom}</a>`);


_renderers.push(
    (x, h, r) => h == "DepenseListPage.Depenses.Result",
    (x, h, r) => renderArrayAsTable(x, h, r, null));


// PageLogs

_renderers.push(
    (x, h, r) => h.startsWith("HomePage.Pages.Result["),
    (x, h, r) => `<a href="${href(x.Link)}">${x.PageName}</a>`);

_renderers.push(
    (x, h, r) => h.startsWith("HomePage.Entities.Result["),
    (x, h, r) => `<a href="${href(x.Link)}">${x.EntityName}</a>`);
_renderers.push(
    (x, h, r) => h.startsWith("HomePage.Tests.Result["),
    (x, h, r) => `<a href="${href(x.Link)}">${x.TestName}</a>`);


_renderers.push(
    (x, h, r) => h == "PagePage.PageReport.Result.RepositoryItems",
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

_renderers.push(
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

_renderers.push(
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

_renderers.push(
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

