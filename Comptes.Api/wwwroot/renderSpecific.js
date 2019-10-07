
_renderers.push(
    (x, id, r) => id == 'TestPageResponse.PageGraph.Result',
    (x, id, r) => {
        r.afterRender(() => setTimeout(() =>
            drawInGraph(
                _page.TestReport.Result.Path.Items,
                document.getElementById('TestPageResponse.PageGraph.Result.Svg'))
            , 1000));
        return `${x.Svg}`.replace('<svg', `<svg id="${id + '.Svg'}"`);
    }
);
