$(function(){

  $('.tabContent > div:first-of-type').fadeIn()

  $('.tab a').on('click',function(e){
    e.preventDefault();
    var $this = $(this),
        tabGroup = '#'+$this.parents('.tab').data('tabgroup'),
        others = $this.closest('li').siblings(),
        target = $this.attr('href');
    others.removeClass('active');
    $this.parents('li').addClass('active');
    $(tabGroup).children('div').hide();
    $(target).fadeIn()
  })
})