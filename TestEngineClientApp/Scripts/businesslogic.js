
var data= [
            {   qid:1,
                qname:"Full form of HTML?",
                opt1:"HyperText markUp Language",
                opt2:"Hyper text martin language",
                opt3:"Hyper text manage language",
                opt4:"Hyper Text middle Language",
                ans:1
             },
             { 
                qid:2,
                qname:"CSS Full Form?",
                opt1:"Caseete style sheet",
                opt2:"Concorde styling language",
                opt3:"Cascading Style Sheet",
                opt4:"Cinnnam Style Sheet",
                ans:3
             },
              {   
                qid:3,
                qname:"Chief Minister of Karnataka",
                opt1:"HD Kumaraswamy",
                opt2:"Rahul",
                opt3:"DK Shivakumar",
                opt4:"Siddramayaa",
                ans:1
             },
              {   
                qid:4,
                qname:"Capital city of Karnataka",
                opt1:"Delhi",
                opt2:"Bangalore",
                opt3:"Kolkota",
                opt4:"Chennai",
                ans:2
             },
              {   
                qid:5,
                qname:"Capital city of Maharashtra",
                opt1:"Delhi",
                opt2:"Mumbai",
                opt3:"Kolkota",
                opt4:"Chennai",
                ans:2
             }
          ];

    var input = [];
    var index=0;
	$(function() {
		
		document.getElementById("resultVal").hidden = true;
		document.getElementById("errorMsg").hidden = true;
		startTask();
	});
    function startTask(){
        index=0;
        for(var i=0;i<data.length;++i){
            input[i]=0;
        }
        var q= data[index];
		
        document.getElementById("question").innerText=q.qid+". "+q.qname;
        document.getElementById("op1").innerText=q.opt1;
        document.getElementById("op2").innerText=q.opt2;
        document.getElementById("op3").innerText=q.opt3;
        document.getElementById("op4").innerText=q.opt4;
        findChecked();
    }


    function Navprev(){
        if(index==0){
            index=data.length-1;    
        }else{
            index--;
        }
        var q= data[index];
        document.getElementById("question").innerText=q.qid+". "+q.qname;
        document.getElementById("op1").innerText=q.opt1;
        document.getElementById("op2").innerText=q.opt2;
        document.getElementById("op3").innerText=q.opt3;
        document.getElementById("op4").innerText=q.opt4;
        findChecked();
    }
	
	function clearSelection(){
		input [index]=0;
 		findChecked();
	}
  

     function next(){
        if(index==data.length-1){
            index=0;    
        }else{
            index++;
        }
        var q= data[index];
        document.getElementById("question").innerText=q.qid+". "+q.qname;
        document.getElementById("op1").innerText=q.opt1;
        document.getElementById("op2").innerText=q.opt2;
        document.getElementById("op3").innerText=q.opt3;
        document.getElementById("op4").innerText=q.opt4;
        findChecked();
    }
  


    function getAnswer1(){
        input [index]=1;    
    }
    function getAnswer2(){
        input [index]=2;    
    }
    function getAnswer3(){
        input [index]=3;    
    }
    function getAnswer4(){
        input [index]=4;    
    }
	



    function findChecked(){
        switch (input[index]){
            case 1:  document.getElementById("1").checked=true; break;
            case 2:  document.getElementById("2").checked=true; break;
            case 3:  document.getElementById("3").checked=true; break;
            case 4:  document.getElementById("4").checked=true; break;
            default:  document.getElementById("1").checked=false;
                      document.getElementById("2").checked=false;
                      document.getElementById("3").checked=false;
                      document.getElementById("4").checked=false;
                      break;  
        }        
    }
	
    function calculate(){
		
        var total = data.length;
		var correct = 0;
		var wrong = 0;
		var notAnswered = 0;
		
		for(var i = 0;i < total; ++i){
			if(input[i] == 0){
				++notAnswered;
			}else if(data[i].ans == input[i]){
				++correct;
			}else{
				++wrong;
			}
		}

		// document.getElementById("rt").innerText=total;
		// document.getElementById("rc").innerText=correct;
		// document.getElementById("rw").innerText=wrong;
		// document.getElementById("rn").innerText=notAnswered;
		
		var yourAddress = document.getElementById("yourAddress").value;
		var submitedResultObj = {
							TotalCount: total,
							CorrectAnswers:correct,
							WrongAnswers:wrong,
							UnAnswered:notAnswered				
						};
		var result = {	
				SubmittedResult:JSON.stringify(submitedResultObj),
				YourAddress:yourAddress//"0x985e2a488544C4B6e7d31AcF25c1f9aE663f9A49"
		}
	
	blockChainIntegration(result);
		
	}
	var AjaxJSON = function(url, type ,data, callback) {
			return jQuery.ajax({
			headers: { 
				'Accept': 'application/json',
				'Content-Type': 'application/json' 
			},
			'type': type,
			'url': url,
			'data': JSON.stringify(data),
			'dataType': 'json',
			'success': callback
			});
	};
	
	function blockChainIntegration(result){
		var urlDNS = "http://manthan.excelindia.com/innovations/submissionapi/";
		console.log(result["YourAddress"]);
		AjaxJSON(urlDNS+'api/Assessment/PostSubmission'
				,"POST"
				,result
				, function(data, status){
					AjaxJSON(urlDNS+'api/Assessment/GetSubmission/'+result["YourAddress"]
							,"GET"
							,""
							, function(data, status){
								if(data && status == "success"){
									document.getElementById("rt").innerText=data.TotalCount ? data.TotalCount : "N/A";
									document.getElementById("rc").innerText=data.CorrectAnswers ? data.CorrectAnswers : "N/A";
									document.getElementById("rw").innerText=data.WrongAnswers ? data.WrongAnswers : "N/A";
									document.getElementById("rn").innerText=data.UnAnswered ? data.UnAnswered : "N/A";
									document.getElementById("resultVal").hidden = false;
									document.getElementById("errorMsg").hidden = true;
								}
								else
								{
									document.getElementById("resultVal").hidden = true;
									document.getElementById("errorMsg").hidden = false;
								}
								focusDivElement();
								
							}).error(function() { 
									document.getElementById("resultVal").hidden = true;
									document.getElementById("errorMsg").hidden = false;
									focusDivElement();
									// document.getElementById('resultDiv').focus();
									});
				}).error(function() { 
						document.getElementById("resultVal").hidden = true;
						document.getElementById("errorMsg").hidden = false;
						focusDivElement();
						// document.getElementById('resultDiv').focus();
						});
		
		
	}
	
	function focusDivElement(){
		goToByScroll("resultDiv");
		$("#resultDiv").focusin(function(){
									$(this).css("background-color", "#FFFFCC");
								});
		
	}
	function goToByScroll(id){
			  // Scroll
			$('html,body').animate({
				scrollTop: $("#"+id).offset().top},
				'slow');
		}

    
	
	