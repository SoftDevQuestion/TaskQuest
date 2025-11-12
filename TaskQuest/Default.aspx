<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TaskQuest.Default" %>

<!DOCTYPE html>

<html class="no-js" xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="x-ua-compatible" content="ie=edge" />
    <title>TaskQuest Web App</title>
    <meta name="description" content="" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <!-- Place favicon.ico in the root directory -->

    <!-- ========================= CSS here ========================= -->
    <link rel="stylesheet" href="assets/css/bootstrap-5.0.0-beta1.min.css" />
    <link rel="stylesheet" href="assets/css/LineIcons.2.0.css"/>
    <link rel="stylesheet" href="assets/css/tiny-slider.css"/>
    <link rel="stylesheet" href="assets/css/animate.css"/>
    <link rel="stylesheet" href="assets/css/lindy-uikit.css"/>
</head>
<body>
    <form id="form1" runat="server">
        <!-- ========================= preloader start ========================= -->
    <div class="preloader">
      <div class="loader">
        <div class="spinner">
          <div class="spinner-container">
            <div class="spinner-rotator">
              <div class="spinner-left">
                <div class="spinner-circle"></div>
              </div>
              <div class="spinner-right">
                <div class="spinner-circle"></div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
    <!-- ========================= preloader end ========================= -->

    <!-- ========================= hero-section-wrapper-6 start ========================= -->
    <section id="home" class="hero-section-wrapper-5">

      <!-- ========================= header-6 start ========================= -->
      <header class="header header-6">
        <div class="navbar-area">
          <div class="container">
            <div class="row align-items-center">
              <div class="col-lg-12">
                <nav class="navbar navbar-expand-lg">
                  <a class="navbar-brand" href="index.html">
                    <img src="assets/img/logo/logo.svg" alt="Logo" />
                  </a>
                  <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarSupportedContent6" aria-controls="navbarSupportedContent6" aria-expanded="false" aria-label="Toggle navigation">
                    <span class="toggler-icon"></span>
                    <span class="toggler-icon"></span>
                    <span class="toggler-icon"></span>
                  </button>
  
                  <div class="collapse navbar-collapse sub-menu-bar" id="navbarSupportedContent6">
                    <ul id="nav6" class="navbar-nav ms-auto">
                      <li class="nav-item">
                        <a class="page-scroll active" href="#home">Home</a>
                      </li>
                      <li class="nav-item">
                        <a class="page-scroll" href="#feature">Feature</a>
                      </li>
                      <li class="nav-item">
                        <a class="page-scroll" href="#about">About</a>
                      </li>

                      <li class="nav-item">
                        <a class="page-scroll" href="#pricing">Use Cases</a>
                      </li>
                      <li class="nav-item">
                        <a class="page-scroll" href="#contact">Contact</a>
                      </li>
                    </ul>
                  </div>
                  
                  <div class="header-action d-flex">
                    <a href="Login.aspx"> <i class="lni lni-user"></i> </a>
                    
                  </div>
                  <!-- navbar collapse -->
                </nav>
                <!-- navbar -->
              </div>
            </div>
            <!-- row -->
          </div>
          <!-- container -->
        </div>
        <!-- navbar area -->
      </header>
      <!-- ========================= header-6 end ========================= -->

      <!-- ========================= hero-5 start ========================= -->
      <div class="hero-section hero-style-5 img-bg" style="background-image: url('assets/img/hero/hero-5/hero-bg.svg')">
        <div class="container">
          <div class="row">
            <div class="col-lg-6">
              <div class="hero-content-wrapper">
                <h2 class="mb-30 wow fadeInUp" data-wow-delay=".2s">Stay Organized .</br> Get Things Done.</h2>
                <p class="mb-30 wow fadeInUp" data-wow-delay=".4s">TaskQuest helps you manage your tasks clarity and focus . Plan smarter , track pregress , and achieve more - effortlessly .</p>
                <a href="Login.aspx" class="button button-lg radius-50 wow fadeInUp" data-wow-delay=".6s">Get Started Now <i class="lni lni-chevron-right"></i> </a>
              </div>
            </div>
            <div class="col-lg-6 align-self-end">
              <div class="hero-image wow fadeInUp" data-wow-delay=".5s">
                <img src="assets/img/hero/hero-5/hero-img.svg" alt="">
              </div>
            </div>
          </div>
        </div>
      </div>
      <!-- ========================= hero-5 end ========================= -->

    </section>
    <!-- ========================= hero-section-wrapper-6 end ========================= -->
		

            <!-- ========================= feature style-5 start ========================= -->
    <section id="feature" class="feature-section feature-style-5">
      <div class="container">
        <div class="row justify-content-center">
          <div class="col-xxl-5 col-xl-5 col-lg-7 col-md-8">
            <div class="section-title text-center mb-60">
              <h3 class="mb-15 wow fadeInUp" data-wow-delay=".2s">Powerful Features to Keep You on Track</h3>
              <p class="wow fadeInUp" data-wow-delay=".4s">Everything you need to organize your day, stay focused, and reach your goals — all in one simple app.</p>
            </div>
          </div>
        </div>

        <div class="row">
          
          <div class="col-lg-4 col-md-6">
            <div class="single-feature wow fadeInUp" data-wow-delay=".4s">
              <div class="icon">
                <i class="lni lni-pallet"></i>
                <svg width="110" height="72" viewBox="0 0 110 72" fill="none" xmlns="http://www.w3.org/2000/svg">
                  <path d="M110 54.7589C110 85.0014 85.3757 66.2583 55 66.2583C24.6243 66.2583 0 85.0014 0 54.7589C0 24.5164 24.6243 0 55 0C85.3757 0 110 24.5164 110 54.7589Z" fill="#EBF4FF"/>
                  </svg> 
              </div>
              <div class="content">
                <h5>Minimal & Clean Design</h5>
                <p>Focus on your goals without distractions — a simple interface that makes productivity feel good.</p>
              </div>
            </div>
          </div>
          <div class="col-lg-4 col-md-6">
            <div class="single-feature wow fadeInUp" data-wow-delay=".6s">
              <div class="icon">
                <i class="lni lni-stats-up"></i>
                <svg width="110" height="72" viewBox="0 0 110 72" fill="none" xmlns="http://www.w3.org/2000/svg">
                  <path d="M110 54.7589C110 85.0014 85.3757 66.2583 55 66.2583C24.6243 66.2583 0 85.0014 0 54.7589C0 24.5164 24.6243 0 55 0C85.3757 0 110 24.5164 110 54.7589Z" fill="#EBF4FF"/>
                  </svg> 
              </div>
              <div class="content">
                <h5>Progress Tracking</h5>
                <p>See how much you’ve accomplished and stay motivated with simple progress visuals.</p>
              </div>
            </div>
          </div>
          <div class="col-lg-4 col-md-6">
            <div class="single-feature wow fadeInUp" data-wow-delay=".2s">
              <div class="icon">
                <i class="lni lni-code-alt"></i>
                <svg width="110" height="72" viewBox="0 0 110 72" fill="none" xmlns="http://www.w3.org/2000/svg">
                  <path d="M110 54.7589C110 85.0014 85.3757 66.2583 55 66.2583C24.6243 66.2583 0 85.0014 0 54.7589C0 24.5164 24.6243 0 55 0C85.3757 0 110 24.5164 110 54.7589Z" fill="#EBF4FF"/>
                  </svg> 
              </div>
              <div class="content">
                <h5>Cross-Device Sync</h5>
                <p>Access your tasks anytime, anywhere — your lists stay updated on all your devices.</p>
              </div>
            </div>
          </div>
          
        </div>

      </div>
    </section>
    <!-- ========================= feature style-5 end ========================= -->

       <!-- ========================= about style-4 start ========================= -->
    <section id="about" class="about-section about-style-4">
      <div class="container">
        <div class="row align-items-center">
          <div class="col-xl-5 col-lg-6">
            <div class="about-content-wrapper">
              <div class="section-title mb-30">
                <h3 class="mb-25 wow fadeInUp" data-wow-delay=".2s">Who We Are & Why We Built Task Quest ?</h3>
                <p class="wow fadeInUp" data-wow-delay=".3s">
                    We are the team behind Task Quest, a group of software enthusiasts passionate about creating smart and simple tools to make everyday life more organized.

With a background in software development, we aim to turn productivity into something enjoyable and easy to achieve.



We built Task Quest because:
                </div>
              <ul>
                <li class="wow fadeInUp" data-wow-delay=".35s">
                  <i class="lni lni-checkmark-circle"></i>
                  We believe staying organized shouldn’t feel complicated — it should feel empowering.
                </li>
                <li class="wow fadeInUp" data-wow-delay=".4s">
                  <i class="lni lni-checkmark-circle"></i>
                  We wanted to help people stay focused and motivated on their daily goals.
                </li>
                <li class="wow fadeInUp" data-wow-delay=".45s">
                  <i class="lni lni-checkmark-circle"></i>
                  We love building tools that bring structure, clarity, and a bit of fun to everyday tasks.
                </li>
              </ul>
              <a  href="Login.aspx" class="button button-lg radius-10 wow fadeInUp" data-wow-delay=".5s">Learn More</a>
            </div>
          </div>
          <div class="col-xl-7 col-lg-6">
            <div class="about-image text-lg-right wow fadeInUp" data-wow-delay=".5s">
              <img src="assets/img/about/about-4/about-img.svg" alt="">
            </div>
          </div>
        </div>
      </div>
    </section>
    <!-- ========================= about style-4 end ========================= -->

        <!-- ========================= pricing style-4 start ========================= -->
    <section id="pricing" class="pricing-section pricing-style-4 bg-light">
      <div class="container">
        <div class="row align-items-center">
          <div class="col-xl-5 col-lg-6">
            <div class="section-title mb-60">
              <h3 class="mb-15 wow fadeInUp" data-wow-delay=".2s">How People Use Task Quest ?</h3>
              <p class="wow fadeInUp" data-wow-delay=".4s">Everyone has a different way of staying organized — and Task Quest fits them all.
From students to professionals to anyone juggling daily life, Task Quest helps you manage your time, stay focused, and make every day feel a little lighter.</p>
            </div>
          </div>
          <div class="col-xl-7 col-lg-6">
            <div class="pricing-active-wrapper wow fadeInUp" data-wow-delay=".4s">
              <div class="pricing-active">
                <div class="single-pricing-wrapper">
                  <div class="single-pricing">
                    <h6>For Students</h6>
                    <h4> 🎓</h4>
                    <h3>Stay on top of your studies</h3>
                    <ul>
                      <li>Keep track of homework, projects, and exams</li>
                      <li>Plan study sessions with smart reminders</li>
                      <li>Balance school and free time with ease</li>
                    </ul>
                    <a href="Login.aspx" class="button radius-30">Get Organized</a>
                  </div>
                </div>
                <div class="single-pricing-wrapper">
                  <div class="single-pricing">
                    <h6>For Professionals</h6>
                    <h4>💼</h4>
                    <h3>Manage work like a pro</h3>
                    <ul>
                      <li>Organize your tasks and meetings in one place</li>
                      <li>Prioritize what really matters and plan for that</li>
                      <li>Boost focus and productivity every day</li>
                    </ul>
                    <a href="Login.aspx"  class="button radius-30">Start Now</a>
                  </div>
                </div>
                <div class="single-pricing-wrapper">
                  <div class="single-pricing">
                    <h6>For Everyday Life</h6>
                    <h4>🏠</h4>
                    <h3>Simplify your routine</h3>
                    <ul>
                      <li>Create to-do lists for daily chores or goals</li>
                      <li>Build positive habits and stay consistent</li>
                      <li>Feel good checking things off your list</li>
                    </ul>
                    <a href="Login.aspx"  class="button radius-30">Try It Today</a>
                  </div>
                </div>
                
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>
    <!-- ========================= pricing style-4 end ========================= -->
        
    <!-- ========================= contact-style-3 start ========================= -->
    <section id="contact" class="contact-section contact-style-3">
      <div class="container">
        <div class="row justify-content-center">
          <div class="col-xxl-5 col-xl-5 col-lg-7 col-md-10">
            <div class="section-title text-center mb-50">
              <h3 class="mb-15">Get in touch</h3>
              <p>Have a question, suggestion, or just want to say hello?
We’re here to help and always open to hearing from you.

Contact us through the form below or via email — we’ll get back to you as soon as possible.</p>
            </div>
          </div>
        </div>
        <div class="row">
          

          <div class="left-wrapper">
              <div class="row">
                <div class="col-lg-12 col-md-6">
                  <div class="single-item">
                    <div class="icon">
                      <i class="lni lni-phone"></i>
                    </div>
                    <div class="text">
                      <p>+982155003335</p>
                      <p>+989106996586</p>
                    </div>
                  </div>
                </div>
                <div class="col-lg-12 col-md-6">
                  <div class="single-item">
                    <div class="icon">
                      <i class="lni lni-envelope"></i>
                    </div>
                    <div class="text">
                      <p>https://www.shariaty.ac.ir/</p>
                      <p>Elahemahmoodi81@gmail.com</p>
                    </div>
                  </div>
                </div>
                <div class="col-lg-12 col-md-6">
                  <div class="single-item">
                    <div class="icon">
                      <i class="lni lni-map-marker"></i>
                    </div>
                    <div class="text">
                      <p>Tehran Province, Tehran, District 19، Tondgooyan Expy, st,61st St، Misagh, Daneshgah St , Shariaty Technical and Vocational College</p>
                    </div>
                  </div>
                </div>
              </div>
              
            </div>
        </div>
      </div>
    </section>
    <!-- ========================= contact-style-3 end ========================= -->

    



        <!-- ========================= footer style-4 start ========================= -->
    <footer class="footer footer-style-4">
      <div class="container">
        
        <div class="copyright-wrapper wow fadeInUp" data-wow-delay=".2s">
          <p>Design and Developed by <a href="https://github.com/SoftDevQuestion/TaskQuest" rel="nofollow" target="_blank">TaskQuest Team</a>.
        </div>
      </div>
    </footer>
    <!-- ========================= footer style-4 end ========================= -->

    <!-- ========================= scroll-top start ========================= -->
    <a href="#" class="scroll-top"> <i class="lni lni-chevron-up"></i> </a>
    <!-- ========================= scroll-top end ========================= -->

    <!-- ========================= JS here ========================= -->
    <script src="assets/js/bootstrap-5.0.0-beta1.min.js"></script>
    <script src="assets/js/tiny-slider.js"></script>
    <script src="assets/js/wow.min.js"></script>
    <script src="assets/js/main.js"></script>
    </form>
</body>
</html>
