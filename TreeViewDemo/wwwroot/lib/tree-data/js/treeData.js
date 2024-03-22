'use strict';

function TreeData(data, select) {
    var main = document.querySelector(select);
    var treecanvas = document.createElement('ul');
    treecanvas.className = 'tree';

    var treeCode = buildTree(data, Object.keys(data)[0]);
    treecanvas.innerHTML = treeCode;
    main.appendChild(treecanvas);

    setTimeout(() => {
        document.querySelector('ul.tree').style.width = '5000%';
        $('.input-edit').removeClass('hidden').hide();
        $('.svg-add-icon, .svg-edit-icon').hide();

        if (editMode) {
            $(document).on('click', 'li', function (event) {
                event.stopPropagation(); // Stop propagation to prevent bubbling
                if ($(this).hasClass('active-i')) return;
                $('.active-i').removeClass('active-i');
                $('.svg-add-icon, .svg-edit-icon').hide();
                $('.input-edit').hide();
                $('.input-display').show();
                //$(this).hide();
                // Show child SVGs within the hovered li
                $(this).children('.svg-add-icon').show();
                $(this).children('.svg-edit-icon').show();
                $(this).children('.input-edit').show();
                $(this).children('.input-display').hide();
                $(this).addClass('active-i');
            });
        }
    }, 500);
}

function buildTree(obj, node) {
    var v = obj[node];
    var treeString = '';
    if (v.attrs) {
        if (editMode) {
            treeString = `<li data-id="${v.id}" data-pid="${v.parentId}">
                        <span class="svg-add-icon hidden" onclick="window.location.assign('/Categories/Create?ParentId=${v.id}&partial=true')">
                            <i class="fa-solid fa-plus"></i>
                        </span>
                        <a style="color:${v.color || 'black'}; background-color:${v.bgColor}" data-bs-toggle="tooltip" title="${v.attrs[0]}&#10;${v.attrs[1]}&#10;${v.attrs[2]}&#10;${v.attrs[3]}&#10;"> 
                            <input name="value" value="${v.value}" class="input-edit hidden" /> 
                             ${(v.logoUrl != null ? '<img src="' + v.logoUrl + '" class="node-image" alt="-"/>' : '')}
                            <span class="input-display">${obj[node].value}</span>
                            </a>
                            <span class="svg-edit-icon" onclick="showEditModal(this)">
                                <i class="fa-solid fa-pen-to-square"></i>
                            </span>`;
        } else {
            treeString = `<li data-id="${v.id}" data-pid="${v.parentId}">
                        <a style="color:${v.color || 'black'}; background-color:${v.bgColor}" data-bs-toggle="tooltip" title="${v.attrs[0]}&#10;${v.attrs[1]}&#10;${v.attrs[2]}&#10;${v.attrs[3]}&#10;"> 
                            ${(v.logoUrl != null ? '<img src="' + v.logoUrl + '" class="node-image" alt="-"/>' : '')}
                            <span class="input-display">${obj[node].value}</span>
                        </a>`;
        }
    } else {
        if (editMode) {
            treeString = `<li data-id="${v.id}" data-pid="${v.parentId}">
                        <span class="svg-add-icon">
                            <i class="fa-solid fa-plus"></i>
                        </span>
                        <a style="color:${v.color || 'black'}; background-color:${v.bgColor}"> 
                            <input name="value" value="${v.value}" class="input-edit" /> 
                            ${(v.logoUrl != null ? '<img src="' + v.logoUrl + '" class="node-image" alt="-"/>' : '')}
                            <span class="input-display">${obj[node].value}</span>
                        </a>
                        <span class="svg-edit-icon" onclick="showEditModal(this)">
                            <i class="fa-solid fa-pen-to-square"></i>
                        </span>`;
        } else {
            treeString = `<li data-id="${v.id}" data-pid="${v.parentId}">
                        <a style="color:${v.color || 'black'}; background-color:${v.bgColor}"> 
                            ${(v.logoUrl != null ? '<img src="' + v.logoUrl + '" class="node-image" alt="-"/>' : '')}
                            <span class="input-display">${obj[node].value}</span>
                        </a>`;
        }
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

setTimeout(() => {
    const link = document.createElement('link');
    link.rel = 'stylesheet';
    link.href = '/css/zoom-dragging.css';
    document.head.appendChild(link);
}, 1000);

function showEditModal($this) {
    if (!editMode) return;
    let id = $($this).closest('li').data('id');
    let pid = $($this).closest('li').data('pid');
    let url = "/Categories/Edit/" + id + '?p=true';
    console.log(url);
    $("#edit-modal-body").load(url, function (res) {
        $('[id=Partial]').val('true');
        const myModal = new bootstrap.Modal(document.getElementById('edit-modal'));
        myModal.show();
    });
    console.log('edit modal');
}

// $(document).on('submit', '#category-edit-from', function(e){
//     e.preventDefault();
//     let data = new FormData(this);
//     $.post('/categories/edit', data, function(res){
//         console.log('res');
//     })
// });

// $(document).ready(function() {
//     $(document).on('mouseenter', 'li', function(event) {
//         $('img').css('transform', 'scale(1.1)');
//         // Check if the hovered element is the one that the handler was bound to
//         if (event.target === this) {
//             $(this).children('a:first').children('img:first').css('transform', 'scale(5)');
//         }
//     }).on('mouseleave', 'li', function(event) {
//         $('img').css('transform', 'scale(1.1)');
//     });
// });
