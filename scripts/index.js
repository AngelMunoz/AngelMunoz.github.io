document.addEventListener('DOMContentLoaded', () => {

  const burger = document.querySelector("#burger");
  const menusin = document.querySelector("#menusin");
  burger.addEventListener('click', () => {
    burger.classList.toggle("is-active");
    menusin.classList.toggle("is-active");
  });

});