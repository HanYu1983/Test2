package main

import (
	"context"
	"log"
	"time"

	"github.com/chromedp/chromedp"
)

func main() {
	var err error

	// create context
	ctxt, cancel := context.WithCancel(context.Background())
	defer cancel()

	// create chrome instance
	c, err := chromedp.New(ctxt, chromedp.WithLog(log.Printf))
	if err != nil {
		log.Fatal(err)
	}

	// run task list
	err = c.Run(ctxt, login())
	if err != nil {
		log.Fatal(err)
	}

	// shutdown chrome
	err = c.Shutdown(ctxt)
	if err != nil {
		log.Fatal(err)
	}

	// wait for chrome to finish
	err = c.Wait()
	if err != nil {
		log.Fatal(err)
	}
}

func login() chromedp.Tasks {
	username := "aass1491"
	return chromedp.Tasks{
		chromedp.Navigate(`https://0736268923-fcs.cp168.ws/login`),
		chromedp.WaitVisible(`input[name="account"]`),
		chromedp.SetValue(`input[name="account"]`, username, chromedp.ByQuery),
		chromedp.SetValue(`input[name="password"]`, username, chromedp.ByQuery),
		chromedp.WaitVisible(`a[href="index"]`),
		chromedp.Click(`a[href="index"]`, chromedp.NodeVisible),

		chromedp.WaitVisible(`.notice_yellow animate`),
		chromedp.WaitVisible(`a[href="load?lottery=BJPK10&page=gy"]`),
		chromedp.Click(`a[href="load?lottery=BJPK10&page=gy"]`, chromedp.ByQuery),
		/*
			chromedp.WaitVisible(`#notice_button1`),
			chromedp.Click(`#notice_button1`, chromedp.NodeVisible),
			chromedp.WaitVisible(`#notice_button2`),
			chromedp.Click(`#notice_button2`, chromedp.NodeVisible),
			chromedp.WaitVisible(`#notice_button3`),
			chromedp.Click(`#notice_button3`, chromedp.NodeVisible),
			chromedp.WaitVisible(`#notice_button4`),
			chromedp.Click(`#notice_button4`, chromedp.NodeVisible),
		*/
		chromedp.Sleep(150 * time.Second),
	}
}
