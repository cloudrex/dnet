﻿using DNet.Builders;
using DNet.ClientStructures;
using DNet.Core;
using DNet.Structures.Channels;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace DNet.Structures.Messages
{
    public class GenericMessage : ClientInjectable
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("channel_id")]
        public string ChannelId { get; set; }

        [JsonProperty("author")]
        public User Author { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }

        [JsonProperty("edited_timestamp")]
        public string EditedTimestamp { get; set; }

        [JsonProperty("mention_everyone")]
        public bool MentionsEveryone { get; set; }

        [JsonProperty("mentions")]
        public MessageReaction[] Mentions { get; set; }

        [JsonProperty("attachments")]
        public MessageAttachment[] Attachments { get; set; }

        [JsonProperty("embeds")]
        public Embed[] Embeds { get; set; }

        [JsonProperty("reactions")]
        public MessageReaction?[] Reactions { get; set; }

        [JsonProperty("nonce")]
        public string Nonce { get; set; }

        [JsonProperty("pinned")]
        public bool Pinned { get; set; }

        [JsonProperty("webhook_id")]
        public string WebhookId { get; set; }

        [JsonProperty("type")]
        public MessageType Type { get; set; }

        [JsonProperty("activity")]
        public MessageActivity? Activity { get; set; }

        [JsonProperty("application")]
        public MessageApplication? Application { get; set; }

        public virtual Channel Channel => this.Client.channels[this.ChannelId];

        public bool? Editable
        {
            get
            {
                if (this.Client == null || !this.Client.User.HasValue)
                {
                    return null;
                }

                return this.Client.User.Value.Id == this.Author.Id;
            }
        }

        public Task<Message> Edit(MessageEdit edit)
        {
            if (this.Client == null || this.Editable == null || this.Editable == false)
            {
                return null;
            }

            return this.Client.toolbox.EditMessage(this.ChannelId, this.Id, edit);
        }

        public Task<Message> Edit(string content)
        {
            return this.Edit(new MessageEdit()
            {
                Content = content
            });
        }

        public Task<Message> Edit(Embed embed)
        {
            return this.Edit(new MessageEdit()
            {
                Embed = embed
            });
        }

        public Task<Message> Edit(RichEmbed richEmbed)
        {
            return this.Edit(richEmbed.Build());
        }
    }
}
