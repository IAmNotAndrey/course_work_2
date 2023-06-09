$(document).ready(function () {
    // Обработка нажатия по элементу дерева
    $('.treeline').on('click', '.tree-node', function (event) {
        event.stopPropagation();

        var selectedNode = $(this);
        var selectedNodeId = selectedNode.data('nodeid');
        var selectedNodeName = selectedNode.data('nodename');
        var selectedNodeDescription = selectedNode.data('nodedescription');

        $('#node-name').text(selectedNodeName);
        $('#node-description').text(selectedNodeDescription);

        // Скрыть все кнопки внутри дерева
        $('.add-button, .delete-button').hide();
        // Показать кнопки только для выбранной вершины
        $(this).find('.add-button, .delete-button').show();
        // Скрыть кнопки потомков текущей выбранной вершины
        $(this).find('.tree-node .add-button, .tree-node .delete-button').hide();

        localStorage.setItem('selectedNodeId', selectedNodeId);
    });
});
