'use strict';

let zoomValue = localStorage.getItem('zoom-value') || 100;
var zoomElement = document.getElementById('zoom-value');
function TreeData(data, select) {
    var main = document.querySelector(select);
    var treecanvas = document.createElement('ul');
    treecanvas.className = 'tree';

    var treeCode = buildTree(data, Object.keys(data)[0]);
    treecanvas.innerHTML = treeCode;
    main.appendChild(treecanvas);

    document.querySelector('ul.tree').style.width = zoomValue + '%';
}

function buildTree(obj, node) {
    var v = obj[node];
    var treeString = '';
    if (v.attrs) {
        treeString = `<li><a style="color:${v.color || 'black'}; background-color:${v.bgColor}" data-bs-toggle="tooltip" title="${v.attrs[0]}&#10;${v.attrs[1]}&#10;${v.attrs[2]}&#10;${v.attrs[3]}&#10;" href='/Categories/Create?ParentId=${v.id}'>${obj[node].value}</a>`;
    } else {
        treeString = `<li><a href='#'>${obj[node].value}</a>`;
    }
    var sons = [];
    for (var i in obj) {
        if (obj[i].parent == node)
            sons.push(i);
    }
    if (sons.length > 0) {
        treeString += "<ul>";
        for (var i in sons) {
            treeString += buildTree(obj, sons[i]);
        }
        treeString += "</ul>";
    }
    return treeString;
}


if (zoomElement) {
    zoomElement.value = zoomValue;
    zoomElement.addEventListener('input', function () {
        document.querySelector('ul.tree').style.width = this.value + '%';
        localStorage.setItem('zoom-value', this.value);
    });
}
