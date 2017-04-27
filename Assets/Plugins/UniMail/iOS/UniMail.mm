#import "UnityAppController.h"
#import "UnityAppController+ViewHandling.h"
#import "UnityAppController+Rendering.h"
#import <MessageUI/MessageUI.h>
#import <MessageUI/MFMailComposeViewController.h>

@interface MailPlugin : UIViewController <MFMailComposeViewControllerDelegate>{}

-(void) sendMailWithImage:(NSString*)to Subject:(NSString*)subject Body:(NSString *)body ImagePath:(NSString *)imagePath;

-(void) sendMail:(NSString*)to Subject:(NSString*)subject Body:(NSString *)body;

-(void) mailComposeController:(MFMailComposeViewController *)controller didFinishWithResult:(MFMailComposeResult)result error:(NSError *)error;

@end

@implementation MailPlugin
-(void) sendMailWithImage:(NSString*)to Subject:(NSString*)subject Body:(NSString *)body ImagePath:(NSString *)imagePath
{
    if (![MFMailComposeViewController canSendMail]) return;
    
    MFMailComposeViewController *mailViewController = [[MFMailComposeViewController alloc] init];
    mailViewController.mailComposeDelegate = self;
    
    // subject
    [mailViewController setSubject:subject];
    
    // mail to
    NSArray *toRecipients = [NSArray arrayWithObject:to];
    [mailViewController setToRecipients:toRecipients];
    
    // body
    [mailViewController setMessageBody:body isHTML:NO];
    
    // image
    UIImage *image= [[UIImage alloc] initWithContentsOfFile:imagePath];
    NSData *data  = [[NSData alloc] initWithData:UIImagePNGRepresentation(image)];
    [mailViewController addAttachmentData:data mimeType:@"image/png" fileName:@"screen_shot.png"];
    
    [GetAppController().rootViewController presentViewController: mailViewController animated:YES completion:NULL];
}

-(void) sendMail:(NSString*)to Subject:(NSString*)subject Body:(NSString *)body
{
    if (![MFMailComposeViewController canSendMail]) return;
    
    MFMailComposeViewController *mailViewController = [[MFMailComposeViewController alloc] init];
    mailViewController.mailComposeDelegate = self;
    
    // subject
    [mailViewController setSubject:subject];
    
    // mail to
    NSArray *toRecipients = [NSArray arrayWithObject:to];
    [mailViewController setToRecipients:toRecipients];
    
    // body
    [mailViewController setMessageBody:body isHTML:NO];
    
    [GetAppController().rootViewController presentViewController: mailViewController animated:YES completion:NULL];
}

-(void) mailComposeController:(MFMailComposeViewController *)controller didFinishWithResult:(MFMailComposeResult)result error:(NSError *)error{
    [GetAppController().rootViewController dismissViewControllerAnimated:YES completion:NULL];
}
@end

NSString* Mail_CreateNSString(const char* string)
{
    return [NSString stringWithUTF8String: string ? string : ""];
}

extern UIViewController* UnityGetGLViewController();
static MailPlugin *mailPlugin = NULL;

extern "C"
{
    void _SendWithImage(const char* charMailTo, const char* charSubject, const char* charBody, const char* charImagePath)
    {
        NSString *mailTo = Mail_CreateNSString(charMailTo);
        NSString *subject = Mail_CreateNSString(charSubject);
        NSString *body = Mail_CreateNSString(charBody);
        NSString *imagePath = Mail_CreateNSString(charImagePath);
        
        if(mailPlugin == NULL) mailPlugin = [[MailPlugin alloc] init];
        
        [mailPlugin sendMailWithImage:mailTo Subject:subject Body:body ImagePath:imagePath];
    }
    
    void _Send(const char* charMailTo, const char* charSubject, const char* charBody)
    {
        NSString *mailTo = Mail_CreateNSString(charMailTo);
        NSString *subject = Mail_CreateNSString(charSubject);
        NSString *body = Mail_CreateNSString(charBody);
        
        if(mailPlugin == NULL) mailPlugin = [[MailPlugin alloc] init];
        
        [mailPlugin sendMail:mailTo Subject:subject Body:body];
    }
}
