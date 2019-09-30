
addRenderer(
    (x, h) => h == "HomePage.Pages.[]",
    (x, h) => `<a href="${x.Link.href}">${x.PageName}</a>`);