$(document).ready(function () {
	// Логика развёртывания / свёртывания элементов дерева
	$(function () {
		var ul = document.querySelectorAll('.treeline > li:not(:only-child) ul, .treeline ul ul');
		for (var i = 0; i < ul.length; i++) {
			var div = document.createElement('div');
			div.className = 'drop';
			div.innerHTML = '+';
			ul[i].parentNode.insertBefore(div, ul[i].previousSibling);
			div.onclick = function () {
				this.innerHTML = (this.innerHTML == '+' ? '−' : '+');
				this.className = (this.className == 'drop' ? 'drop dropM' : 'drop');
			}
		}
	});

	//$(".tree-node").click(function (event) {
	//	event.stopPropagation();

	//	// Удаляем класс "selected" у предыдущего выбранного элемента
	//	$('.tree-node.selected-tree-node').removeClass('selected-tree-node');

	//	var node = $(this);
	//	// Добавляем класс "selected-tree-node" только к родительскому элементу
	//	node.addClass('selected-tree-node');
	//});

	//// Обработка нажатия по элементу дерева
	//$('.treeline').on('click', '.tree-node', function (event) {

	//	event.stopPropagation();

	//	// Удаляем класс "selected" у предыдущего выбранного элемента
	//	$('.treeline .tree-node.selected-tree-node').removeClass('selected-tree-node');

	//	var node = $(this);
	//	// Добавляем класс "selected-tree-node" только к родительскому элементу
	//	node.closest('.tree-node').addClass('selected-tree-node');
	//});

	//// Обработка наведения курсора на элемент дерева
	//$('.treeline').on('mouseenter', '.tree-node', function (event) {
	//	event.stopPropagation();

	//	$(this).css('background-color', 'grey');
	//});

	//$('.treeline').on('mouseleave', '.tree-node', function (event) {
	//	event.stopPropagation();

	//	$(this).css('background-color', '');
	//});
});
