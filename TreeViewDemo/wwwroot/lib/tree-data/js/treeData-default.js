'use strict';

function TreeData(data, select) {
    var main = document.querySelector(select);
    var treecanvas = document.createElement('ul');
    treecanvas.className = 'tree';
    var treeCode = buildTree(data, Object.keys(data)[0]);
    treecanvas.innerHTML = treeCode;
    main.appendChild(treecanvas);

    var firstLi = treecanvas.querySelector('li');
    if (firstLi) {
        var liHeight = firstLi.clientHeight; // Get the height of the first li
        treecanvas.style.height = liHeight + 'px'; // Set the ul height to match
    }
}

function buildTree(obj, node) {
    var v = obj[node];
    var treeString = `<li data-id="${v.id}">
                        <a style="color:${v.color || 'black'}; background-color:${v.bgColor}"> 
                            ${(v.logoUrl != null ? '<img src="' + v.logoUrl + '" class="node-image" alt="-"/>' : '')}
                            <span class="input-display">${obj[node].value}</span>
                        </a>`;
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
