$(document).ready(function () {
    $('.list-group-item').click(function () {
        // Удалить класс "selected" у всех элементов списка
        $('.list-group-item').removeClass('selected');

        // Добавить класс "selected" только выбранному элементу
        $(this).addClass('selected');
    });
});