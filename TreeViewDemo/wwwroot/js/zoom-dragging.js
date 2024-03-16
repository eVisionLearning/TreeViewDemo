modelId = modelId || 0;
let lastX, lastY;
let zoomFactor = localStorage.getItem('zoomFactor_' + modelId) ? parseFloat(localStorage.getItem('zoomFactor_' + modelId)) : 1;
let storedZoomFactor;
let isDragging = false;
let map = document.getElementById('tree-map');

map.style.transformX = localStorage.getItem('transformX_' + modelId) ? parseFloat(localStorage.getItem('transformX_' + modelId)) : 350;
map.style.transformY = localStorage.getItem('transformY_' + modelId) ? parseFloat(localStorage.getItem('transformY_' + modelId)) : 60;

// Apply the saved transform on load
map.style.transform = `scale(${zoomFactor}) translate(${map.style.transformX}px, ${map.style.transformY}px)`;

// Function to handle mouse down event
map.addEventListener('mousedown', (event) => {
    isDragging = true;
    storedZoomFactor = zoomFactor;
    lastX = event.clientX;
    lastY = event.clientY;
    event.preventDefault(); // Prevent default behavior
    map.classList.add('dragging');
});

document.addEventListener('mouseup', () => {
    isDragging = false;
    map.classList.remove('dragging');
});

// Function to handle mouse move event
document.addEventListener('mousemove', (event) => {
    if (isDragging) {
        const deltaX = event.clientX - lastX;
        const deltaY = event.clientY - lastY;

        // Update transform with stored zoom and round transformX value
        map.style.transform = `scale(${storedZoomFactor}) translate(${Math.round(map.style.transformX)}px, ${Math.round(map.style.transformY)}px)`;
        map.style.transformX = parseInt(map.style.transformX) + deltaX / storedZoomFactor;
        map.style.transformY = parseInt(map.style.transformY) + deltaY / storedZoomFactor;

        lastX = event.clientX;
        lastY = event.clientY;
    }
});

// Function to handle page scroll event (zooming)
map.addEventListener('wheel', (event) => {
    const deltaY = event.deltaY;
    const zoomDelta = deltaY > 0 ? 0.02 : -0.02; // Adjust zoom delta based on scroll direction (adjust as needed)
    zoomFactor += zoomDelta;

    // Restrict zoom factor within a reasonable range
    zoomFactor = Math.max(0.5, Math.min(4, zoomFactor));

    map.style.transform = `scale(${zoomFactor}) translate(${Math.round(map.style.transformX)}px, ${Math.round(map.style.transformY)}px)`;

    // Update storedZoomFactor after zooming
    storedZoomFactor = zoomFactor;
});

// Save the state when the user leaves the page
window.addEventListener('beforeunload', () => {
    localStorage.setItem('zoomFactor_' + modelId, zoomFactor);
    localStorage.setItem('transformX_' + modelId, map.style.transformX);
    localStorage.setItem('transformY_' + modelId, map.style.transformY);
});

setTimeout(() => {
    $('body').removeClass('opacity0');
}, 1000)
// function clearLocalStorage(){
//     localStorage.clear();
// }

$(document).on('dblclick', '.input-display', function (e){
   console.log(e);
   $(this).hide();
   $(this).prev('input').show();
});